﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Collections.Pooled.Generic.Internals.Unsafe
{
    public readonly ref struct TempHashSetInternalsRefUnsafe<T>
    {
#if TARGET_64BIT || PLATFORM_ARCH_64 || UNITY_64
        [NonSerialized] public readonly ulong FastModMultiplier;
#endif

        [NonSerialized] public readonly int Count;
        [NonSerialized] public readonly int FreeList;
        [NonSerialized] public readonly int FreeCount;
        [NonSerialized] public readonly int Version;
        [NonSerialized] public readonly bool ClearEntries;

        [NonSerialized] public readonly Span<int> Buckets;
        [NonSerialized] public readonly Span<Entry<T>> Entries;
        [NonSerialized] public readonly IEqualityComparer<T> Comparer;

        public TempHashSetInternalsRefUnsafe(in TempHashSet<T> source)
        {
#if TARGET_64BIT || PLATFORM_ARCH_64 || UNITY_64
            FastModMultiplier = source._fastModMultiplier;
#endif

            Count = source._count;
            FreeList = source._freeList;
            FreeCount = source._freeCount;
            Version = source._version;
            ClearEntries = TempHashSet<T>.s_clearEntries;
            Buckets = source._buckets;
            Entries = source._entries;
            Comparer = source._comparer;
        }
    }

    partial class TempCollectionInternalsUnsafe
    {
        /// <summary>
        /// Returns a structure that holds references to internal fields of <paramref name="source"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TempHashSetInternalsRefUnsafe<T> GetRef<T>(
                in TempHashSet<T> source
            )
            => new TempHashSetInternalsRefUnsafe<T>(source);

        /// <summary>
        /// Returns the internal <see cref="Entry{T}"/> array as a <see cref="Span{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<Entry<T>> AsSpan<T>(
                in TempHashSet<T> source
            )
            => source._entries.AsSpan(0, source._count);

        /// <summary>
        /// Gets either a ref to a <typeparamref name="T"/> in the <see cref="TempHashSet{T}"/> or a ref null if it does not exist in the <paramref name="set"/>.
        /// </summary>
        /// <param name="set">The set to get the ref to <typeparamref name="T"/> from.</param>
        /// <param name="equalValue">The value to search for.</param>
        /// <remarks>
        /// Items should not be added or removed from the <see cref="TempHashSet{T}"/> while the ref <typeparamref name="T"/> is in use.
        /// The ref null can be detected using System.Runtime.CompilerServices.Unsafe.IsNullRef
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetValueRefOrNullRef<T>(
                in TempHashSet<T> set, T equalValue
            ) where T : notnull
            => ref set.FindValue(equalValue);

        /// <summary>Adds the specified element to the set if it's not already contained.</summary>
        /// <param name="value">The element to add to the set.</param>
        /// <param name="location">The index into <see cref="_entries"/> of the element.</param>
        /// <returns>
        /// true if the element is added to the <see cref="TempHashSet{T}"/> object; false if the element is already present.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddIfNotPresent<T>(
                ref TempHashSet<T> set, T value, out int location
            )
            => set.AddIfNotPresent(value, out location);
    }
}