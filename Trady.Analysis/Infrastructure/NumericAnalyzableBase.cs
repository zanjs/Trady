﻿using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class NumericAnalyzableBase<TInput, TMappedInput, TOutput>
        : AnalyzableBase<TInput, TMappedInput, decimal?, TOutput>, INumericAnalyzable<TOutput>
    {
        protected NumericAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper) : base(inputs, inputMapper)
        {
        }

        #region IDiffAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeDiff(int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(Diff, startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeDiff(IEnumerable<int> indexes) => Compute(Diff, indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourDiff(int index) => Compute(Diff, index);

        public TOutput Diff(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISmaAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeSma(int periodCount, int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(i => Sma(periodCount, i), startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeSma(int periodCount, IEnumerable<int> indexes) => Compute(i => Sma(periodCount, i), indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourSma(int periodCount, int index) => Compute(i => Sma(periodCount, i), index);

        public TOutput Sma(int periodCount, int index)
            => index >= periodCount - 1 ? ComputeAndMap(i => Enumerable.Range(i - periodCount + 1, periodCount).Select(ComputeByIndex).Average(), index) : default(TOutput);

        #endregion

        #region IEmaAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeEma(int periodCount, int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(i => Ema(periodCount, i), startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeEma(int periodCount, IEnumerable<int> indexes) => Compute(i => Ema(periodCount, i), indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourEma(int periodCount, int index) => Compute(i => Ema(periodCount, i), index);

        public TOutput Ema(int periodCount, int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMemaAnalyzable implementation

        public IReadOnlyList<TOutput> ComputeMema(int periodCount, int? startIndex = default(int?), int? endIndex = default(int?)) => Compute(i => Mema(periodCount, i), startIndex, endIndex);

        public IReadOnlyList<TOutput> ComputeMema(int periodCount, IEnumerable<int> indexes) => Compute(i => Mema(periodCount, i), indexes);

        public (TOutput Prev, TOutput Current, TOutput Next) ComputeNeighbourMema(int periodCount, int index) => Compute(i => Mema(periodCount, i), index);

        public TOutput Mema(int periodCount, int index) => throw new NotImplementedException();

        #endregion
    }
}
