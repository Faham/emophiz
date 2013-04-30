using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Util;

namespace SensorLib.Filters
{

    /// <summary>
    /// Represents a filter's transfer function. H(z) = b0 + b1*z^(-1) + b2*z^(-2)  /  ( a0 + a1*z^(-1) + a2*z(-1) )
    /// </summary>
    public struct TransferFunction
    {
        public TransferFunction(double[] b, double[] a)
        {
            this.b = b;
            this.a = a;
            return;
        }

        /// <summary>
        /// Gets the numerator coefficients of the transfer function.
        /// </summary>
        public double[] B { get { return b; } }

        /// <summary>
        /// Gets the denominator coefficients of the transfer function.
        /// </summary>
        public double[] A { get { return a; } }


        readonly double[] b;
        readonly double[] a;
    }

}
