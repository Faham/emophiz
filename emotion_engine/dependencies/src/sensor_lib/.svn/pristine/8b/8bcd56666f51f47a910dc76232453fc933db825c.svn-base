using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SensorLib.Filters
{
    /// <summary>
    /// A filter chain is a filter which is made up of multiple cascading filters.
    /// </summary>
    public class FilterChain : Filter
    {
        public FilterChain(Filter[] filters)
        {
            if (filters == null)
                throw new ArgumentException("Filter array cannot be null.");

            this.filters = filters;
            return;
        }

        public Filter[] Filters { get { return filters; } }

        public override bool IsRealTime { get { return filters.All(x => x.IsRealTime); } }

        public override void Reset()
        {
            Array.ForEach(filters, x => x.Reset());
            return;
        }

        public override float[] FilterData(float[] input)
        {
            if (input == null)
                return new float[0];

            float[] data = input;
            float[] data2;

            //!!Could maybe speed (and memory) this up by just modifying the data in-place instead of creating a new array each time
            for (int i = 0; i < filters.Length; i++)
            {
                data2 = filters[i].FilterData(data);
                data = data2;
            }

            return data;
        }


        readonly Filter[] filters;
    }
}
