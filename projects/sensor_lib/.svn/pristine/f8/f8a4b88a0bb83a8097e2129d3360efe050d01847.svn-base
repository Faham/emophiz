using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;
using SensorLib.Util;

namespace SensorLib.Filters.RealTime
{
    class FlatFilter : IirFilter
    {
        public FlatFilter() : base(new ZeroPoleGain(gain, zeros, poles))
        { return; }

        readonly static float gain = 1f;
        readonly static Complex[] zeros = { Complex.One };
        readonly static Complex[] poles = { Complex.Zero };


    }
}
