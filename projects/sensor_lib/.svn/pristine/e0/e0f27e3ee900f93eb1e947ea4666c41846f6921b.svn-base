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
    class PeakHearHPF : IirFilter
    {

        public PeakHearHPF() : base(0.7837, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000   -1.9997    1.0000    1.0000   -1.8210    0.8296"),
            new Sos("1.0000   -1.9977    1.0000    1.0000   -1.8432    0.8537"),
            new Sos("1.0000   -1.9948    1.0000    1.0000   -1.8864    0.8999"),
            new Sos("1.0000   -1.9928    1.0000    1.0000   -1.9478    0.9637")
        };   

    }
}
