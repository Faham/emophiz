using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// Notch filter to take out 60Hz power noise -- 
    /// Chebyshev II SBF -- 
    /// w1 stop =  |
    /// w1 pass =  | 
    /// w2 pass =  |
    /// w2 stop =  |
    /// delta pass = 1e-3 |
    /// delta stop = 1e-3 |
    /// gain pass = 1 |
    /// gain stop = 0
    /// </summary>
    class PowerNotchFilter : IirFilter
    {

        public PowerNotchFilter() : base(0.8976, sections)
        { return; }


        readonly static Sos[] sections =
        {
            new Sos("1.0000   -1.9680    1.0038    1.0000   -1.9028    0.9353"),
            new Sos("1.0000   -1.9610    0.9960    1.0000   -1.9002    0.9414"),
            new Sos("1.0000   -1.9752    1.0046    1.0000   -1.9276    0.9535"),
            new Sos("1.0000   -1.9602    0.9998    1.0000   -1.9275    0.9759"),
            new Sos("1.0000   -1.9668    0.9958    1.0000   -1.9605    0.9833")
        };
        
    }
}
