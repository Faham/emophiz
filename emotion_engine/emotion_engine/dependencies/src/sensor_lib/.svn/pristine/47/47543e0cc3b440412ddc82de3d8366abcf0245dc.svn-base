using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// Chebyshev II BPF -- 
    /// w1 pass = pi/8 | 
    /// w2 pass = pi/2 |
    /// transitions = pi/16 |
    /// delta pass = 1e-3 |
    /// delta stop = 1e-3 |
    /// gain pass = 1 |
    /// gain stop = 0
    /// </summary>
    class DeltaBrainBPF : IirFilter
    {

        public DeltaBrainBPF() : base(0.0198, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000    1.9605    1.0000    1.0000    0.6147    0.1063"),
            new Sos("1.0000    1.6860    1.0000    1.0000    0.5623    0.1788"),
            new Sos("1.0000    1.2944    1.0000    1.0000    0.4719    0.3015"),
            new Sos("1.0000    0.9315    1.0000    1.0000    0.3663    0.4477"),
            new Sos("1.0000    0.6588    1.0000    1.0000    0.2683    0.6001"),
            new Sos("1.0000    0.4844    1.0000    1.0000    0.1953    0.7534"),
            new Sos("1.0000   -1.9994    1.0000    1.0000   -1.7654    0.7799"),
            new Sos("1.0000   -1.9951    1.0000    1.0000   -1.7781    0.7964"),
            new Sos("1.0000   -1.9877    1.0000    1.0000   -1.8033    0.8280"),
            new Sos("1.0000   -1.9791    1.0000    1.0000   -1.8349    0.8665"),
            new Sos("1.0000   -1.9712    1.0000    1.0000   -1.8635    0.9021"),
            new Sos("1.0000    0.4005    1.0000    1.0000    0.1602    0.9133"),
            new Sos("1.0000   -1.9652    1.0000    1.0000   -1.8974    0.9418"),
            new Sos("1.0000   -1.9620    1.0000    1.0000   -1.9328    0.9805")
        };

    }
}
