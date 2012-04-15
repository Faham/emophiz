using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using SensorLib.Util;

namespace SensorLib.Filters.RealTime
{
    class SingleZeroHPF : IirFilter
    {
        public SingleZeroHPF() : base(gain, sections)
        { return; }

        static double gain = 1.0f;
        static Complex[] zeros = { Complex.One, -Complex.One, Complex.Zero };
        static Complex[] poles = { Complex.One, Complex.Zero, Complex.Zero };

        //This has to be below variables it uses!
        readonly static Sos[] sections =
        {
            new Sos(zeros, poles)
        };

    }
}
