using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Util;

namespace SensorLib.Filters.RealTime
{
    class SingleZeroLPF : IirFilter
    {
        public SingleZeroLPF() : base(gain, sections)
        { return; }

        static double gain = 1.0;
        static Complex[] zeros = { Complex.One, Complex.One, Complex.Zero };
        static Complex[] poles = { Complex.One, Complex.Zero, Complex.Zero };


        readonly static Sos[] sections =
        {
            new Sos(zeros, poles)
        };

        
    }
}
