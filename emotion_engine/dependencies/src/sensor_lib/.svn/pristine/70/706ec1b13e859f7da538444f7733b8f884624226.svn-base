using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    class DeltaFilter : FilterChain
    {

        public DeltaFilter() : base(1, filterChain)
        { return; }


        static readonly Filter[] filterChain = 
        {
            new AntiAliasBrainLPF16(),
            new DownSampleFilter(16),
            new AntiAliasBrainLPF8(),
            new DownSampleFilter(8),
            new DeltaBrainBPF()
        };
        
    }
}
