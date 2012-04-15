using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Filters;
using SensorLib.Filters.FilterCreation;

namespace SensorLib.Filters.RealTime
{

    /// <summary>
    /// Downsamples the data, applying an anti-aliasing filter first.
    /// </summary>
    public class DecimationFilter : Filter
    {
        /// <summary>
        /// Applies a 10th-order Butterworth filter.
        /// </summary>
        /// <param name="factor"></param>
        public DecimationFilter(int factor) : this(factor, 5, false)
        { return; }

        /// <summary>
        /// Applies a nth-order Butterworth filter.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="filterOrder"></param>
        public DecimationFilter(int factor, int filterOrder, bool bidirectional)
        {
            this.factor = factor;

            Util.Pair<double,double> cornerFreqs = new Util.Pair<double,double>(Math.PI / factor, 0.0);
            FilterOrderSpec spec = FilterOrderSpec.CreateButterworthSpec(cornerFreqs, filterOrder, BandType.LowPass);

            Filter antiAliasFilter = FilterFactory.CreateIirFilter(spec);
            if(bidirectional)
                antiAliasFilter = new BidirectionalFilter(antiAliasFilter);

            Filter downSampleFilter = new DownSampleFilter(factor);
            filter = new FilterChain(new Filter[] { antiAliasFilter, downSampleFilter });
            return;
        }


        public int Factor { get { return factor; } }


        public override bool IsRealTime { get { return true; } }

        public override void Reset()
        {
            filter.Reset();
            return;
        }

        public override float[] FilterData(float[] data)
        {
            return filter.FilterData(data);
        }

        readonly Filter filter;
        readonly int factor;
    }
}
