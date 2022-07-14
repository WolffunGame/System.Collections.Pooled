﻿using System;
using System.Runtime.CompilerServices;

namespace Collections.Pooled.Generic.Internals.Unsafe
{
    public readonly ref struct ValueQueueInternalsRefUnsafe<T>
    {
        [NonSerialized] public readonly int Head;
        [NonSerialized] public readonly int Tail;
        [NonSerialized] public readonly int Size;
        [NonSerialized] public readonly int Version;
        [NonSerialized] public readonly bool ClearArray;
        [NonSerialized] public readonly Span<T> Array;

        public ValueQueueInternalsRefUnsafe(in ValueQueue<T> source)
        {
            Head = source._head;
            Tail = source._tail;
            Size = source._size;
            Version = source._version;
            ClearArray = ValueQueue<T>.s_clearArray;
            Array = source._array;
        }
    }

    partial class ValueCollectionInternalsUnsafe
    {
        /// <summary>
        /// Returns a structure that holds references to internal fields of <paramref name="source"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueQueueInternalsRefUnsafe<T> GetRef<T>(
                in ValueQueue<T> source
            )
            => new ValueQueueInternalsRefUnsafe<T>(source);

        /// <summary>
        /// Returns the internal array as a <see cref="Span{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(
                in ValueQueue<T> source
            )
            => source._array.AsSpan(0, source._size);
    }
}
