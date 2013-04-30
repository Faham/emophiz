using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SensorLib.Filters.FilterCreation
{
    public static class Windows
    {

        public enum Type
        {
            Rectangular,
            Bartlett,
            Hann,
            Hamming,
            Blackman
        }


        public static double[] CreateWindow(int order, Type windowType)
        {
            //Create window
            double[] window;
            switch (windowType)
            {
                case Windows.Type.Rectangular: window = Windows.CreateRectangular(order); break;
                case Windows.Type.Bartlett: window = Windows.CreateBartlett(order); break;
                case Windows.Type.Hann: window = Windows.CreateHann(order); break;
                case Windows.Type.Hamming: window = Windows.CreateHamming(order); break;
                case Windows.Type.Blackman: window = Windows.CreateBlackman(order); break;
                default: Debug.Assert(false); throw new NotImplementedException();
            }

            return window;
        }

        /// <summary>
        /// Creates a rectangular window.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static double[] CreateRectangular(int order)
        {
            return Enumerable.Repeat<double>(1, order + 1).ToArray();
        }

        /// <summary>
        /// Creates a Bartlett (triangular) window.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static double[] CreateBartlett(int order)
        {
            double[] window = new double[order + 1];

            int i = 0;
            for (; i < window.Length / 2 + 1; i++)
            {
                window[i] = 2 * i / order;
            }

            for (; i < window.Length; i++)
            {
                window[i] = 2 * (1 - i/order);
            }

            return window;
        }

        /// <summary>
        /// Create a Hanning window.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static double[] CreateHann(int order)
        {
            return CalculateWindow(order, 0.5, 0.5, 0);
        }


        /// <summary>
        /// Create a Hamming window.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static double[] CreateHamming(int order)
        {
            return CalculateWindow(order, 0.54, 0.46, 0);
        }


        /// <summary>
        /// Create a Blackman window.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static double[] CreateBlackman(int order)
        {
            return CalculateWindow(order, 0.42, 0.5, 0.08);
        }


        /// <summary>
        /// Creates a symmetric window based on a summation of cosines.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="a0"></param>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        static double[] CalculateWindow(int order, double a0, double a1, double a2)
        {

            //Initialize the window
            double[] window = new double[order + 1];

            //Find the halfway point of the window
            int half = (int)Math.Ceiling(order / 2.0);

            //Calculate the window
            int windowLength = window.Length;
            for (int i = 0; i < half; i++)
            {
                window[i] = a0 + a1 * Math.Cos(2 * Math.PI * i / order) + a2 * Math.Cos(4*Math.PI*i/order);
                window[windowLength-1 - i] = window[i];
            }

            return window;
        }

    }
}
