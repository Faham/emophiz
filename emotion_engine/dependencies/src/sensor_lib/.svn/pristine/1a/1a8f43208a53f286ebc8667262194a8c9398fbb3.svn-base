using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;
using SensorLib.Util;

namespace SensorLib.Filters
{
    /// <summary>
    /// Filter design specification using the filter order.
    /// </summary>
    [Serializable]
    public class FilterOrderSpec
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cornerFreqs">The corner frequencies in radians/sample.  Only the first element is used in a low/high pass filter.</param>
        /// <param name="order"></param>
        /// <param name="bandType"></param>
        /// <returns></returns>
        public static FilterOrderSpec CreateButterworthSpec(Pair<double,double> cornerFreqs, int order, BandType bandType)
        {
            FilterOrderSpec spec = new FilterOrderSpec();
            spec.CornerFreqs = cornerFreqs;
            spec.Order = order;
            spec.Ripple = 0.0;
            spec.BandType = bandType;
            spec.FilterType = IirFilterType.Butterworth;

            return spec;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cornerFreqs">The corner frequencies in radians/sample.  Only the first element is used in a low/high pass filter.</param>
        /// <param name="order"></param>
        /// <param name="ripple"></param>
        /// <param name="bandType"></param>
        /// <returns></returns>
        public static FilterOrderSpec CreateChebyshevType1Spec(Pair<double, double> cornerFreqs, int order, double ripple, BandType bandType)
        {
            FilterOrderSpec spec = new FilterOrderSpec();
            spec.CornerFreqs = cornerFreqs;
            spec.Order = order;
            spec.Ripple = ripple;
            spec.BandType = bandType;
            spec.FilterType = IirFilterType.ChebyshevType1;

            return spec;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cornerFreqs">The corner frequencies in radians/sample.  Only the first element is used in a low/high pass filter.</param>
        /// <param name="order"></param>
        /// <param name="ripple"></param>
        /// <param name="bandType"></param>
        /// <returns></returns>
        public static FilterOrderSpec CreateChebyshevType2Spec(Pair<double, double> cornerFreqs, int order, double ripple, BandType bandType)
        {
            FilterOrderSpec spec = new FilterOrderSpec();
            spec.CornerFreqs = cornerFreqs;
            spec.Order = order;
            spec.Ripple = ripple;
            spec.BandType = bandType;
            spec.FilterType = IirFilterType.ChebyshevType2;

            return spec;
        }

        public FilterOrderSpec()
        {
            return;
        }

        public FilterOrderSpec(FilterOrderSpec spec)
        {
            if (spec == null)
                return;
            
            spec.CopyTo(this);
            return;
        }

        public void CopyTo(FilterOrderSpec spec)
        {
            spec.CornerFreqs = new Pair<double,double>(CornerFreqs);
            spec.Order = Order;
            spec.Ripple = Ripple;
            spec.BandType = BandType;
            spec.FilterType = FilterType;
            return;
        }

        /// <summary>
        /// Left and right corner frequencies of the pass band in radians/sample.
        /// Only the first element is used in a low/high pass filter.
        /// </summary>
        public Pair<double, double> CornerFreqs { get; set; }

        /// <summary>
        /// Filter order.  The filter order is the length of the filter minus 1.
        /// The higher the number the sharper the filter but the more computation needed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Ripple in decibels.
        /// Is not used with the Butterworth filter.
        /// </summary>
        public double Ripple { get; set; }

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
