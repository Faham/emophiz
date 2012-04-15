using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;
using SensorLib.Util;

namespace SensorLib.Filters
{
    /// <summary>
    /// Filter design specification using frequencies of the filter bands.
    /// </summary>
    [Serializable]
    public class FilterFrequencySpec
    {

        public FilterFrequencySpec()
        {
            PassFreqs = new Pair<double, double>();
            StopFreqs = new Pair<double, double>();
            return;
        }

        public FilterFrequencySpec(FilterFrequencySpec spec)
        {
            if (spec == null)
            {
                PassFreqs = new Pair<double, double>();
                StopFreqs = new Pair<double, double>();
                return;
            }
            
            spec.CopyTo(this);
            return;
        }

        public void CopyTo(FilterFrequencySpec spec)
        {
            spec.PassFreqs = new Pair<double,double>(PassFreqs);
            spec.StopFreqs = new Pair<double, double>(StopFreqs);
            spec.PassRipple = PassRipple;
            spec.StopRipple = StopRipple;
            spec.FilterType = FilterType;
            spec.BandType = BandType;
            return;
        }

        /// <summary>
        /// Left and right corner frequencies of the pass band in radians/sample.
        /// Only the first element is used in a low/high pass filter.
        /// </summary>
        public Pair<double, double> PassFreqs { get; set; }

        /// <summary>
        /// Left and right corner frequencies of the stop band in radians/sample.
        /// Only the first element is used in a low/high pass filter.
        /// </summary>
        public Pair<double, double> StopFreqs { get; set; }

        /// <summary>
        /// Pass band ripple in decibels.
        /// </summary>
        public double PassRipple { get; set; }

        /// <summary>
        /// Stop band attenuation in decibels.
        /// </summary>
        public double StopRipple { get; set; }

        /// <summary>
        /// IIR filter type.
        /// </summary>
        public IirFilterType FilterType { get; set; }

        /// <summary>
        /// Band type of the filter.
        /// </summary>
        public BandType BandType { get; set; }

    }
}
