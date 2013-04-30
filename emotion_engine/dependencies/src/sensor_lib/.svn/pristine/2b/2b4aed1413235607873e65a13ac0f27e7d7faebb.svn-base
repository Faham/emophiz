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
    class AlphaBrainBPF : IirFilter
    {

        public AlphaBrainBPF() : base(0.0024, sections)
        { return; }

        readonly static Sos[] sections =
        {
            new Sos("1.0000   -0.0000   -1.0000    1.0000   -0.6918    0.2671"),
            new Sos("1.0000    1.2465    1.0000    1.0000   -0.3686    0.3059"),
            new Sos("1.0000   -1.9215    1.0000    1.0000   -1.0648    0.4526"),
            new Sos("1.0000    0.4144    1.0000    1.0000   -0.3003    0.4782"),
            new Sos("1.0000   -1.7856    1.0000    1.0000   -1.2512    0.6257"),
            new Sos("1.0000   -0.0129    1.0000    1.0000   -0.3376    0.6429"),
            new Sos("1.0000   -1.6787    1.0000    1.0000   -1.3460    0.7512"),
            new Sos("1.0000   -0.2282    1.0000    1.0000   -0.3987    0.7724"),
            new Sos("1.0000   -1.6087    1.0000    1.0000   -1.4004    0.8423"),
            new Sos("1.0000   -0.3377    1.0000    1.0000   -0.4525    0.8731"),
            new Sos("1.0000   -1.5674    1.0000    1.0000   -1.4391    0.9121"),
            new Sos("1.0000   -0.3846    1.0000    1.0000   -0.4904    0.9585"),
            new Sos("1.0000   -1.5483    1.0000    1.0000   -1.4756    0.9714")
        };   

    }
}
