using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// Chebyshev II LPF -- 
    /// w pass = 45*pi/1024 |
    /// w stop = pi/16 |
    /// delta pass = 1e-3 |
    /// delta stop = 1e-3 |
    /// gain pass = 1 |
    /// gain stop = 0
    /// </summary>
    class AntiAliasBrainLPF16 : IirFilter
    {
        public AntiAliasBrainLPF16() : base(0.00072287, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000    1.0000         0    1.0000   -0.7253         0"),
            new Sos("1.0000   -1.4206    1.0000    1.0000   -1.5150    0.5833"),
            new Sos("1.0000   -1.8281    1.0000    1.0000   -1.6445    0.6986"),
            new Sos("1.0000   -1.9137    1.0000    1.0000   -1.7597    0.8018"),
            new Sos("1.0000   -1.9435    1.0000    1.0000   -1.8453    0.8795"),
            new Sos("1.0000   -1.9561    1.0000    1.0000   -1.9046    0.9339"),
            new Sos("1.0000   -1.9610    1.0000    1.0000   -1.9507    0.9787")
        };

    }
}
