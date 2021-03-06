﻿// Description: Async extension methods for LINQ (Language Integrated Query).
// Website & Documentation: https://github.com/zzzprojects/LINQ-Async
// Forum: https://github.com/zzzprojects/LINQ-Async/issues
// License: http://www.zzzprojects.com/license-agreement/
// More projects: http://www.zzzprojects.com/
// Copyright (c) 2015 ZZZ Projects. All rights reserved.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HSNXT.Linq;

namespace HSNXT
{
    public static partial class Extensions
    {
        public static Task<int> Min(this Task<IOrderedEnumerable<int>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<int?> Min(this Task<IOrderedEnumerable<int?>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<long> Min(this Task<IOrderedEnumerable<long>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<long?> Min(this Task<IOrderedEnumerable<long?>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<float> Min(this Task<IOrderedEnumerable<float>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<float?> Min(this Task<IOrderedEnumerable<float?>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<double> Min(this Task<IOrderedEnumerable<double>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<double?> Min(this Task<IOrderedEnumerable<double?>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<decimal> Min(this Task<IOrderedEnumerable<decimal>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<decimal?> Min(this Task<IOrderedEnumerable<decimal?>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<TSource> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, Enumerable.Min, cancellationToken);
        }

        public static Task<int> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source, Func<TSource, int> selector,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<int?> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, int?> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<long> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, long> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<long?> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, long?> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<float> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, float> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<float?> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, float?> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<double> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, double> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<double?> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, double?> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<decimal> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, decimal> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<decimal?> Min<TSource>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, decimal?> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }

        public static Task<TResult> Min<TSource, TResult>(this Task<IOrderedEnumerable<TSource>> source,
            Func<TSource, TResult> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.FromTaskEnumerable(source, selector, Enumerable.Min, cancellationToken);
        }
    }
}