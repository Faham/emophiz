using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Util;

namespace SensorLib.Filters
{

    /// <summary>
    /// Represents a filter in zero-pole-gain form.
    /// </summary>
    public struct ZeroPoleGain
    {
        public ZeroPoleGain(double gain, Complex[] zeros, Complex[] poles)
        {
            this.gain = gain;
            this.zeros = zeros;
            this.poles = poles;
            return;
        }

        /// <summary>
        /// The gain (pre-scale) of the filter.
        /// </summary>
        public double Gain { get { return gain; } }

        /// <summary>
        /// Positions of the zeros on a zero-pole plot.
        /// </summary>
        public Complex[] Zeros { get { return zeros; } }

        /// <summary>
        /// Positions of the poles on a zero-pole plot.
        /// </summary>
        public Complex[] Poles { get { return poles; } }


        readonly double gain;
        readonly Complex[] zeros;
        readonly Complex[] poles;
    }

}
