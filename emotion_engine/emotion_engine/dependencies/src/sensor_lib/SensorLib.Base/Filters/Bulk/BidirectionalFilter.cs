using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;


namespace SensorLib.Filters
{
    /// <summary>
    /// A bidirectional filter is a filter that filters the data from both directions.  This makes any zero-pole filter linear-phase.
    /// </summary>
    public class BidirectionalFilter : Filter
    {

        /// <summary>
        /// Creates a bidirectional filter from any filter.
        /// </summary>
        public BidirectionalFilter(Filter filter)
        {
            this.filter = filter;

            return;
        }


        public Filter Filter { get { return filter; } }

        public override bool IsRealTime { get { return false; } }
        
        public override void Reset()
        {
            filter.Reset();
            return;
        }

        /*
        public Sample<float>[] FilterData(Sample<float>[] input)
        {
            //Filter forward through the data
            Sample<float>[] filteredOnceData = filter.FilterData(input);

            //Filter backward through the data
            Sample<float>[] output = filter.FilterData(filteredOnceData.Reverse().ToArray());

            //Return filtered data
            return output;
        }
        */

        public override float[] FilterData(float[] input)
        {
            //Filter forward through the data
            float[] filteredOnceData = filter.FilterData(input);

            //Filter backward through the data
            float[] output = filter.FilterData(filteredOnceData.Reverse().ToArray());

            //Return filtered data
            return output.Reverse().ToArray();
        }


        readonly Filter filter;

        
    }
}
