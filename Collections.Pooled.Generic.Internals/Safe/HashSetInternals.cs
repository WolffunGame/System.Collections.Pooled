﻿using System;
using System.Buffers;
using System.Collections.Generic;

namespace ZBase.Collections.Pooled.Generic.Internals
{
    public readonly struct HashSetInternals<T> : IDisposable
    {
#if TARGET_64BIT || PLATFORM_ARCH_64 || UNITY_64
        [NonSerialized] public readonly ulong FastModMultiplier;
#endif

        [NonSerialized] public readonly int Count;
        [NonSerialized] public readonly int FreeList;
        [NonSerialized] public readonly int FreeCount;
        [NonSerialized] public readonly int Version;
        [NonSerialized] public readonly bool ClearEntries;

        [NonSerialized] public readonly int[] Buckets;
        [NonSerialized] public readonly Entry<T>[] Entries;
        [NonSerialized] public readonly IEqualityComparer<T> Comparer;

        [NonSerialized] public readonly ArrayPool<int> BucketPool;
        [NonSerialized] public readonly ArrayPool<Entry<T>> EntryPool;

        public HashSetInternals(HashSet<T> source)
        {
#if TARGET_64BIT || PLATFORM_ARCH_64 || UNITY_64
            FastModMultiplier = source._fastModMultiplier;
#endif

            Count = source._count;
            FreeList = source._freeList;
            FreeCount = source._freeCount;
            Version = source._version;
            ClearEntries = HashSet<T>.s_clearEntries;
            Buckets = source._buckets;
            Entries = source._entries;
            Comparer = source._comparer;
            BucketPool = source._bucketPool;
            EntryPool = source._entryPool;
        }

        public void Dispose()
        {
            if (Buckets.IsNullOrEmpty() == false)
            {
                try
                {
                    BucketPool?.Return(Buckets);
                }
                catch { }
            }

            if (Entries.IsNullOrEmpty() == false)
            {
                try
                {
                    EntryPool?.Return(Entries, ClearEntries);
                }
                catch { }
            }
        }
    }

    partial class CollectionInternals
    {
        /// <summary>
        /// Returns a structure that holds ownership of internal fields of <paramref name="source"/>.
        /// </summary>
        /// <remarks>
        /// Afterward <paramref name="source"/> will be disposed.
        /// </remarks>
        public static HashSetInternals<T> TakeOwnership<T>(
                HashSet<T> source
            )
        {
            var internals = new HashSetInternals<T>(source);

            source._buckets = null;
            source._entries = null;
            source.Dispose();

            return internals;
        }
    }
}
