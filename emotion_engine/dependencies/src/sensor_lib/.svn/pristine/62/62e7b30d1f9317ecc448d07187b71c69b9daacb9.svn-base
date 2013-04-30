using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters.RealTime;
using SensorLib.Util;

namespace SensorLib.Filters
{

    /// <summary>
    /// Represents a filter in second-order section form with gain.
    /// </summary>
    public struct SosGain
    {
        public SosGain(double gain, Sos[] sections)
        {
            this.gain = gain;
            this.sections = sections;
            return;
        }

        /// <summary>
        /// The gain (pre-scale) of the filter.
        /// </summary>
        public double Gain { get { return gain; } }

        /// <summary>
        /// The second-order sections of the filter.
        /// </summary>
        public Sos[] Sections { get { return sections; } }


        readonly double gain;
        readonly Sos[] sections;
    }

}
