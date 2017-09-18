﻿using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class Percentile<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        public int PeriodCount { get; }
        public decimal Percent { get; }

        public Percentile(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount, decimal percent) : base(inputs, inputMapper)
        {
            Percent = percent;
            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
        {
			if (Percent < 0 || Percent > 1)
				throw new ArgumentException("percent should be between 0 and 1", nameof(Percent));

			if (index < PeriodCount - 1)
				return null;

			var subset = mappedInputs.Skip(index - PeriodCount + 1).Take(PeriodCount).OrderBy(v => v).ToList();
			var idx = Percent * (subset.Count - 1) + 1;

			if (idx == 1) return subset[0];
			if (idx == subset.Count) return subset.Last();

			return subset[(int)idx - 1] + (subset[(int)idx] - subset[(int)idx - 1]) * (idx - (int)idx);
        }
    }

    public class PercentileByTuple : Percentile<decimal, decimal?>
    {
        public PercentileByTuple(IEnumerable<decimal> inputs, int periodCount, decimal percent) 
            : base(inputs, i => i, periodCount, percent)
        {
        }
    }

	public class Percentile : Percentile<Candle, AnalyzableTick<decimal?>>
	{
		public Percentile(IEnumerable<Candle> inputs, int periodCount, decimal percent)
			: base(inputs, i => i.Close, periodCount, percent)
		{
		}
	}
}
