﻿using System;
using System.Runtime.CompilerServices;

namespace ZBase.Collections.Pooled.Generic.Internals
{
    public readonly ref struct ValueListInternalsRef<T>
    {
        [NonSerialized] public readonly int Size;
        [NonSerialized] public readonly int Version;
        [NonSerialized] public readonly bool ClearItems;
        [NonSerialized] public readonly ReadOnlySpan<T> Items;

        public ValueListInternalsRef(in ValueList<T> source)
        {
            Size = source._size;
            Version = source._version;
            ClearItems = ValueList<T>.s_clearItems;
            Items = source._items;
        }
    }

    partial class ValueCollectionInternals
    {
        /// <summary>
        /// Returns a structure that holds references to internal fields of <paramref name="source"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueListInternalsRef<T> GetRef<T>(
                in ValueList<T> source
            )
            => new ValueListInternalsRef<T>(source);

        /// <summary>
        /// Returns the internal array as a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(
                in this ValueList<T> source
            )
            => source._items.AsSpan(0, source._size);

        /// <summary>
        /// Returns the internal array as a <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(
                in this ValueList<T> source
            )
            => source._items.AsMemory(0, source._size);
    }
}
