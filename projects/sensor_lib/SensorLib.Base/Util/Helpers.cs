using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SensorLib.Util
{
    
    class Helpers
    {



        /// <summary>
        /// Converts the roots to polynomial form.  The roots must be two roots which are complex conjugates of each other.
        /// (x - r1)(x - r2) -> x^2 - x(r1 + r2) + r1*r2
        /// </summary>
        public static void RootsToPoly(Complex root1, Complex root2, out double a0, out double a1, out double a2)
        {
            // Make sure the roots are conjugates of each other
            Debug.Assert((root1 + root2).IsRealWithTolerance);

            a0 = 1;
            a1 = -(root1 + root2).Real;
            a2 = (root1 * root2).Real;

            return;
        }


        /// <summary>
        /// Converts polynomial form to the roots.
        /// </summary>
        public static void PolyToRoots(double a0, double a1, double a2, out Complex root1, out Complex root2)
        {
            Complex determinant = new Complex(a1 * a1 - 4 * a0 * a2, 0).SquareRoot;
            root1 = (-a1 + determinant) / (2 * a0);
            root2 = (-a1 - determinant) / (2 * a0);

            // The roots should be complex conjugates of each other
            Debug.Assert((root1 + root2).IsRealWithTolerance);
            return;
        }

        /// <summary>
        /// Finds the convolution of the data.  Is also algebraically equivalent to multiplying the polynomials whose coefficients are specified in the parameters.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double[] Convolution(double[] a, double[] b)
        {
            int m = a.Length;
            int n = b.Length;
            int r = m + n - 1;

            double[] result = new double[r];

            for (int i = 0; i < r; i++)
            {
                result[i] = 0;

                for (int j = Math.Max(0, (i+1)-n); j < Math.Min(m, i+1); j++)
                {
                    result[i] += a[j]*b[i - j];
                }
            }

            return result;
        }


        public static double ComputePolynomial(double[] coefficients, double value)
        {
            return coefficients.Reverse().Select((x,i) => x * Math.Pow(value, i)).Sum();
        }

        public static Complex ComputePolynomial(double[] coefficients, Complex value)
        {
            return coefficients.Reverse().Select((x, i) => Complex.FromPolar(x * Math.Pow(value.Magnitude, i), i * value.Angle)).Aggregate(Complex.Zero, (x, y) => x + y);
        }

    }
}
