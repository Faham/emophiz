using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace SensorLib.Filters.RealTime
{
    class AlphaFilter : FilterChain
    {

        public AlphaFilter() : base(1, filterChain)
        { return; }


        static readonly Filter[] filterChain = 
        {
            new AntiAliasBrainLPF_old(),
            new DownSampleFilter(32),
            new AlphaBrainBPF(),
        };
        
    }
}
