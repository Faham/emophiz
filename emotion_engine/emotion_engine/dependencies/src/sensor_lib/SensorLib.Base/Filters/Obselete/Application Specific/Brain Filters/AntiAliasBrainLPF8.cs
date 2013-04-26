using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// Chebyshev II LPF -- 
    /// w pass = pi/16 |
    /// w stop = pi/8 |
    /// delta pass = 1e-3 |
    /// delta stop = 1e-3 |
    /// gain pass = 1 |
    /// gain stop = 0
    /// </summary>
    class AntiAliasBrainLPF8 : IirFilter
    {
        public AntiAliasBrainLPF8() : base(0.0011f, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000    1.0000         0    1.0000   -0.6533         0"),
            new Sos("1.0000   -0.9890    1.0000    1.0000   -1.3852    0.4973"),
            new Sos("1.0000   -1.6504    1.0000    1.0000   -1.5514    0.6478"),
            new Sos("1.0000   -1.7996    1.0000    1.0000   -1.7148    0.7991"),
            new Sos("1.0000   -1.8432    1.0000    1.0000   -1.8535    0.9336")
        };

    }
}
