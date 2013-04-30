using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// Chebyshev II BPF -- 
    /// w1 stop = 7*pi/32 |
    /// w1 pass = 8*pi/32 | 
    /// w2 pass = 13*pi/32 |
    /// w2 stop = 14*pi/32 |
    /// delta pass = 1e-3 |
    /// delta stop = 1e-3 |
    /// gain pass = 1 |
    /// gain stop = 0
    /// </summary>
    class PeakHearLPF : IirFilter
    {

        public PeakHearLPF() : base(0.00083433, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000    1.0000         0    1.0000   -0.6365         0"),
            new Sos("1.0000   -1.0326    1.0000    1.0000   -1.3492    0.4696"),
            new Sos("1.0000   -1.6922    1.0000    1.0000   -1.5072    0.6036"),
            new Sos("1.0000   -1.8465    1.0000    1.0000   -1.6539    0.7281"),
            new Sos("1.0000   -1.9026    1.0000    1.0000   -1.7560    0.8162"),
            new Sos("1.0000   -1.9278    1.0000    1.0000   -1.8453    0.8966"),
            new Sos("1.0000   -1.9399    1.0000    1.0000   -1.8902    0.9326"),
            new Sos("1.0000   -1.9450    1.0000    1.0000   -1.9382    0.9815")
        };   

    }
}
