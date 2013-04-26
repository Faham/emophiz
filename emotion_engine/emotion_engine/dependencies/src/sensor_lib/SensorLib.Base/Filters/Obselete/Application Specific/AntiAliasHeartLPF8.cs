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
    class AntiAliasHeartLPF8 : IirFilter
    {

        public AntiAliasHeartLPF8() : base(0.0011, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000    1.0000         0    1.0000   -0.6629         0"),
            new Sos("1.0000   -1.0391    1.0000    1.0000   -1.4031    0.5090"),
            new Sos("1.0000   -1.6713    1.0000    1.0000   -1.5661    0.6569"),
            new Sos("1.0000   -1.8120    1.0000    1.0000   -1.7257    0.8048"),
            new Sos("1.0000   -1.8531    1.0000    1.0000   -1.8606    0.9357")
        };   

    }
}
