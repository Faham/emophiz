using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;

namespace SensorLib.Filters.RealTime
{

    
    /// <summary>
    /// A linear filter is made up of zeros and poles in the complex zero-pole plane.
    /// </summary>
    public abstract class LinearFilter : Filter
    {

        public override bool IsRealTime { get { return true; } }

        /// <summary>
        /// Gets the filter in zero-pole-gain form.
        /// </summary>
        public abstract ZeroPoleGain ZeroPoleGain { get; }

        /// <summary>
        /// Gets the filter's transfer function.
        /// </summary>
        public abstract TransferFunction TransferFunction { get; }

        /// <summary>
        /// Returns filter as a bidirectional filter.
        /// </summary>
        /// <returns></returns>
        public BidirectionalFilter AsBidirectional()
        {
            return new BidirectionalFilter(this);
        }

        /// <summary>
        /// Calculates the frequency response of the filter using 512 sample points.
        /// </summary>
        /// <returns>Frequency response of filter.</returns>
        public Complex[] CalculateFreqResponse()
        {
            return CalculateFreqResponse(512);
        }

        /// <summary>
        /// Calculates the frequency response of the filter.
        /// </summary>
        /// <param name="numFreqPoints">Number of sample points in response.</param>
        /// <returns>Frequency response of filter.</returns>
        public abstract Complex[] CalculateFreqResponse(int numFreqPoints);

        /// <summary>
        /// Gives the number of delay elements in the filter.
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// Gives the number of coefficients in the filter.  This is just the filter length + 1.
        /// </summary>
        public int FilterLength { get { return Order + 1; } }

        public abstract bool IsLinearPhase { get; }

        
    }
}
