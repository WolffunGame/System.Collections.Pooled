﻿using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace ZBase.Collections.Pooled.Generic
{
    partial struct TempStack<T>
    {
        public TempStack(T[] items) : this(items.AsSpan(), ArrayPool<T>.Shared)
        { }

        public TempStack(T[] items, ArrayPool<T> pool) : this(items.AsSpan(), pool)
        { }

        public TempStack(in ReadOnlySpan<T> span) : this(span, ArrayPool<T>.Shared)
        { }

        public TempStack(in ReadOnlySpan<T> span, ArrayPool<T> pool)
        {
            _size = default;
            _version = default;
            _pool = pool ?? ArrayPool<T>.Shared;

            int count = span.Length;

            if (count == 0)
            {
                _array = s_emptyArray;
            }
            else
            {
                _array = _pool.Rent(count);
                span.CopyTo(_array);
                _size = count;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(in Span<T> dest)
            => CopyTo(dest, 0, _size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(in Span<T> dest, int destIndex)
            => CopyTo(dest, destIndex, _size);

        public void CopyTo(in Span<T> dest, int destIndex, int count)
        {
            if (destIndex < 0 || destIndex > dest.Length)
            {
                ThrowHelper.ThrowArrayIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLessOrEqual();
            }

            if (count < 0)
            {
                ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum();
            }

            if (dest.Length - destIndex < count || _size < count)
            {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
            }

            Span<T> src = _array.AsSpan(0, _size);

            if (src.Length == 0)
                return;

            int srcIndex = 0;
            int dstIndex = destIndex + count;
            while (srcIndex < count)
            {
                dest[--dstIndex] = src[srcIndex++];
            }
        }

        public void Dispose()
        {
            ReturnArray(s_emptyArray);
            _size = 0;
            _version++;
        }
    }
}