﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using HSNXT.Reactive.Concurrency;
using HSNXT.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System;

namespace HSNXT.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region + Amb +

        public virtual IObservable<TSource> Amb<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Amb_(first, second);
        }

        public virtual IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources)
        {
            return Amb_(sources);
        }

        public virtual IObservable<TSource> Amb<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Amb_(sources);
        }

        private static IObservable<TSource> Amb_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return sources.Aggregate(Observable.Never<TSource>(), (previous, current) => previous.Amb(current));
        }

        private static IObservable<TSource> Amb_<TSource>(IObservable<TSource> leftSource, IObservable<TSource> rightSource)
        {
            return new Amb<TSource>(leftSource, rightSource);
        }

        #endregion

        #region + Buffer +

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(IObservable<TSource> source, Func<IObservable<TBufferClosing>> bufferClosingSelector)
        {
            return new Buffer<TSource, TBufferClosing>.Selector(source, bufferClosingSelector);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(IObservable<TSource> source, IObservable<TBufferOpening> bufferOpenings, Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector)
        {
            return source.Window(bufferOpenings, bufferClosingSelector).SelectMany(ToList);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(IObservable<TSource> source, IObservable<TBufferBoundary> bufferBoundaries)
        {
            return new Buffer<TSource, TBufferBoundary>.Boundaries(source, bufferBoundaries);
        }

        #endregion

        #region + Catch +

        public virtual IObservable<TSource> Catch<TSource, TException>(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler) where TException : Exception
        {
            return new Catch<TSource, TException>(source, handler);
        }

        public virtual IObservable<TSource> Catch<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Catch_<TSource>(new[] { first, second });
        }

        public virtual IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources)
        {
            return Catch_<TSource>(sources);
        }

        public virtual IObservable<TSource> Catch<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Catch_<TSource>(sources);
        }

        private static IObservable<TSource> Catch_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new Catch<TSource>(sources);
        }

        #endregion

        #region + CombineLatest +

        public virtual IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new CombineLatest<TFirst, TSecond, TResult>(first, second, resultSelector);
        }

        /* The following code is generated by a tool checked in to $/.../Source/Tools/CodeGenerators. */

        #region CombineLatest auto-generated code (6/10/2012 7:25:03 PM)

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, IObservable<TSource16> source16, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
        }

        #endregion

        public virtual IObservable<TResult> CombineLatest<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return CombineLatest_<TSource, TResult>(sources, resultSelector);
        }

        public virtual IObservable<IList<TSource>> CombineLatest<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return CombineLatest_<TSource, IList<TSource>>(sources, res => res.ToList());
        }

        public virtual IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
        {
            return CombineLatest_<TSource, IList<TSource>>(sources, res => res.ToList());
        }

        private static IObservable<TResult> CombineLatest_<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return new CombineLatest<TSource, TResult>(sources, resultSelector);
        }

        #endregion

        #region + Concat +

        public virtual IObservable<TSource> Concat<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Concat_<TSource>(new[] { first, second });
        }

        public virtual IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
        {
            return Concat_<TSource>(sources);
        }

        public virtual IObservable<TSource> Concat<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Concat_<TSource>(sources);
        }

        private static IObservable<TSource> Concat_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new Concat<TSource>(sources);
        }

        public virtual IObservable<TSource> Concat<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Concat_<TSource>(sources);
        }

        public virtual IObservable<TSource> Concat<TSource>(IObservable<Task<TSource>> sources)
        {
            return Concat_<TSource>(Select(sources, TaskObservableExtensions.ToObservable));
        }

        private IObservable<TSource> Concat_<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Merge(sources, 1);
        }

        #endregion

        #region + Merge +

        public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Merge_<TSource>(sources);
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<Task<TSource>> sources)
        {
            return new Merge<TSource>.Tasks(sources);
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return Merge_<TSource>(sources, maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return Merge_<TSource>(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations), maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler)
        {
            return Merge_<TSource>(sources.ToObservable(scheduler), maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Merge_<TSource>(new[] { first, second }.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second, IScheduler scheduler)
        {
            return Merge_<TSource>(new[] { first, second }.ToObservable(scheduler));
        }

        public virtual IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
        {
            return Merge_<TSource>(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
        {
            return Merge_<TSource>(sources.ToObservable(scheduler));
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Merge_<TSource>(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, IScheduler scheduler)
        {
            return Merge_<TSource>(sources.ToObservable(scheduler));
        }

        private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return new Merge<TSource>.Observables(sources);
        }

        private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return new Merge<TSource>.ObservablesMaxConcurrency(sources, maxConcurrent);
        }

        #endregion

        #region + OnErrorResumeNext +

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return OnErrorResumeNext_<TSource>(new[] { first, second });
        }

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(params IObservable<TSource>[] sources)
        {
            return OnErrorResumeNext_<TSource>(sources);
        }

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return OnErrorResumeNext_<TSource>(sources);
        }

        private static IObservable<TSource> OnErrorResumeNext_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new OnErrorResumeNext<TSource>(sources);
        }

        #endregion

        #region + SkipUntil +

        public virtual IObservable<TSource> SkipUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
            return new SkipUntil<TSource, TOther>(source, other);
        }

        #endregion

        #region + Switch +

        public virtual IObservable<TSource> Switch<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Switch_<TSource>(sources);
        }

        public virtual IObservable<TSource> Switch<TSource>(IObservable<Task<TSource>> sources)
        {
            return Switch_<TSource>(Select(sources, TaskObservableExtensions.ToObservable));
        }

        private IObservable<TSource> Switch_<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return new Switch<TSource>(sources);
        }

        #endregion

        #region + TakeUntil +

        public virtual IObservable<TSource> TakeUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
            return new TakeUntil<TSource, TOther>(source, other);
        }

        #endregion

        #region + Window +

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(IObservable<TSource> source, Func<IObservable<TWindowClosing>> windowClosingSelector)
        {
            return new Window<TSource, TWindowClosing>.Selector(source, windowClosingSelector);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(IObservable<TSource> source, IObservable<TWindowOpening> windowOpenings, Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector)
        {
            return windowOpenings.GroupJoin(source, windowClosingSelector, _ => Observable.Empty<Unit>(), (_, window) => window);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries)
        {
            return new Window<TSource, TWindowBoundary>.Boundaries(source, windowBoundaries);
        }

        #endregion

        #region + WithLatestFrom +

        public virtual IObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new WithLatestFrom<TFirst, TSecond, TResult>(first, second, resultSelector);
        }

        #endregion

        #region + Zip +

        public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new Zip<TFirst, TSecond, TResult>.Observable(first, second, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return Zip_<TSource>(sources).Select(resultSelector);
        }

        public virtual IObservable<IList<TSource>> Zip<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Zip_<TSource>(sources);
        }

        public virtual IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources)
        {
            return Zip_<TSource>(sources);
        }

        private static IObservable<IList<TSource>> Zip_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new Zip<TSource>(sources);
        }

        /* The following code is generated by a tool checked in to $/.../Source/Tools/CodeGenerators. */

        #region Zip auto-generated code (6/10/2012 8:15:28 PM)

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, IObservable<TSource16> source16, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
        }

        #endregion

        public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return new Zip<TFirst, TSecond, TResult>.Enumerable(first, second, resultSelector);
        }

        #endregion
    }
}
