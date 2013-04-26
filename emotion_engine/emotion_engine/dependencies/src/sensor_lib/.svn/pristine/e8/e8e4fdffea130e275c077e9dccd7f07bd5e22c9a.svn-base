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
    class HeartOffsetHPF : IirFilter
    {

        public HeartOffsetHPF() : base(0.9427, sections)
        { return; }

        /*
        //0.1
        readonly static SOS[] sections =
        {
            new SOS(0.9886f, "1.0000   -1.0000         0    1.0000   -0.9886         0"),
            new SOS(1.0000f, "1.0000   -2.0000    1.0000    1.0000   -1.9885    0.9886")
        };
        */

        /*
        //0.05  !!pretty good! d = pi/64
        readonly static SOS[] sections =
        {
            new SOS(0.9943f, "1.0000   -1.0000         0    1.0000   -0.9943         0"),
            new SOS(1.0000f, "1.0000   -2.0000    1.0000    1.0000   -1.9943    0.9943")
        };
        */

        /*
        //wp = .09, ws = .13
        readonly static SOS[] sections =
        {
            new SOS(0.7140f, "1.0000   -2.1194    1.1245    1.0000   -1.8072    0.8173"),
            new SOS(1.0000f, "1.0000   -2.0790    1.0915    1.0000   -1.8328    0.8468"),
            new SOS(1.0000f, "1.0000   -2.0151    1.0348    1.0000   -1.8743    0.8936"),
            new SOS(1.0000f, "1.0000   -1.8806    0.8854    1.0000   -1.8896    0.8958"),
            new SOS(1.0000f, "1.0000   -1.9041    0.9162    1.0000   -1.9218    0.9429"),
            new SOS(1.0000f, "1.0000   -1.9511    0.9706    1.0000   -1.9521    0.9759")
        };
        */
        
        
        //wp = .06, ws = .02 !!Very good!
        readonly static Sos[] sections =
        {
            new Sos("1.0000   -0.9980         0    1.0000   -0.9741         0"),
            new Sos("1.0000   -2.0035    1.0036    1.0000   -1.9531    0.9538"),
            new Sos("1.0000   -1.9973    0.9975    1.0000   -1.9669    0.9678"),
            new Sos("1.0000   -2.0005    1.0009    1.0000   -1.9873    0.9884")
        };
        


        /*
        //0.4
        readonly static SOS[] sections =
        {
            new SOS(0.4641f, "1.0000   -1.9997    0.9998    1.0000   -1.6811    0.7076"),
            new SOS(1.0000f, "1.0000   -1.9868    1.0015    1.0000   -1.6880    0.7220"),
            new SOS(1.0000f, "1.0000   -1.9782    0.9983    1.0000   -1.6981    0.7465"),
            new SOS(1.0000f, "1.0000   -1.9496    1.0024    1.0000   -1.7183    0.7853"),
            new SOS(1.0000f, "1.0000   -1.9391    0.9976    1.0000   -1.7487    0.8350"),
            new SOS(1.0000f, "1.0000   -1.9165    1.0014    1.0000   -1.7908    0.8944"),
            new SOS(1.0000f, "1.0000   -1.9118    0.9989    1.0000   -1.8474    0.9630")
        };
        */
    }
}
