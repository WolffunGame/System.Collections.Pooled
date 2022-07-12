﻿using System;
using System.Runtime.CompilerServices;

namespace Collections.Pooled.Generic.Internals
{
    public readonly ref struct TempQueueInternalsRef<T>
    {
        [NonSerialized] public readonly int Head;
        [NonSerialized] public readonly int Tail;
        [NonSerialized] public readonly int Size;
        [NonSerialized] public readonly int Version;
        [NonSerialized] public readonly bool ClearArray;
        [NonSerialized] public readonly ReadOnlySpan<T> Array;

        public TempQueueInternalsRef(in TempQueue<T> source)
        {
            Head = source._head;
            Tail = source._tail;
            Size = source._size;
            Version = source._version;
            ClearArray = TempQueue<T>.s_clearArray;
            Array = source._array;
        }
    }

    public static partial class CollectionInternals
    {
        /// <summary>
        /// Returns a structure that holds references to internal fields of <paramref name="source"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TempQueueInternalsRef<T> GetRef<T>(
                TempQueue<T> source
            )
            => new TempQueueInternalsRef<T>(source);

        /// <summary>
        /// Returns the internal array as a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(
                TempQueue<T> source
            )
            => source._array.AsSpan(0, source.Count);
    }
}
