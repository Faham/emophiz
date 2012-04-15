using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// Chebyshev II LPF -- 
    /// w pass = pi/64 |
    /// w stop = pi/32 |
    /// delta pass = 1e-3 |
    /// delta stop = 1e-3 |
    /// gain pass = 1 |
    /// gain stop = 0
    /// </summary>
    class AntiAliasBrainLPF_old : IirFilter
    {
        public AntiAliasBrainLPF_old() : base(0.00036300, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000    1.0000         0    1.0000   -0.9016         0"),
            new Sos("1.0000   -1.9191    1.0000    1.0000   -1.8331    0.8418"),
            new Sos("1.0000   -1.9768    1.0000    1.0000   -1.8900    0.8969"),
            new Sos("1.0000   -1.9872    1.0000    1.0000   -1.9393    0.9450"),
            new Sos("1.0000   -1.9901    1.0000    1.0000   -1.9778    0.9829")  
        };

    }
}
