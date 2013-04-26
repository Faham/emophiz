using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Filters.RealTime;
using SensorLib.Util;

namespace SensorLib.Filters.FilterCreation
{
    public class FilterFactory
    {

        /// <summary>
        /// Creates an infinite-impulse response filter based on corner frequencies of each band.
        /// </summary>
        public static IirFilter CreateIirFilter(FilterFrequencySpec freqSpec)
        {
            FilterOrderSpec orderSpec = OrderSelectors.ConvertFrequencySpecToOrderSpec(freqSpec);
            return CreateIirFilter(orderSpec);
        }

        /// <summary>
        /// Creates a infinite-impulse response filter based on pass-band corner frequencies and order.
        /// </summary>
        public static IirFilter CreateIirFilter(FilterOrderSpec orderSpec)
        {

            //Get analog prototype filter (usually low-pass filter with corner frequency of 1)
            ZeroPoleGain analogPrototype;
            switch (orderSpec.FilterType)
            {
                case IirFilterType.Butterworth:
                    analogPrototype = AnalogPrototypes.Butterworth(orderSpec.Order);
                    break;

                case IirFilterType.ChebyshevType1:
                    analogPrototype = AnalogPrototypes.ChebyshevType1(orderSpec.Order, orderSpec.Ripple);
                    break;

                case IirFilterType.ChebyshevType2:
                    analogPrototype = AnalogPrototypes.ChebyshevType2(orderSpec.Order, orderSpec.Ripple);
                    break;

                default: Debug.Assert(false); throw new Exception();
            }


            return AnalogPrototypeToDigitalFilter(analogPrototype, orderSpec.CornerFreqs, orderSpec.BandType, orderSpec.FilterType);
        }



        #region Windowed FIR Filters

        /// <summary>
        /// Creates a nth-order low-pass Type I linear-phase FIR filter.
        /// </summary>
        /// <param name="freq">Corner frequency in radians.</param>
        /// <param name="order">Filter order.</param>
        /// <param name="windowType">Window type.</param>
        /// <returns></returns>
        public static FirFilter CreateFirFilter(double freq, int order, Windows.Type windowType)
        {
            return CreateFirFilter(freq, order, windowType, true);
        }


        /// <summary>
        /// Creates a nth-order low-pass or high-pass Type I linear-phase FIR filter.
        /// </summary>
        /// <param name="freq">Corner frequency in radians.</param>
        /// <param name="order">Filter order.</param>
        /// <param name="windowType">Window type.</param>
        /// <param name="lowpass">True specifies low-pass, false specifies high-pass.</param>
        /// <returns></returns>
        public static FirFilter CreateFirFilter(double freq, int order, Windows.Type windowType, bool lowpass)
        {
            //Create window
            double[] window = Windows.CreateWindow(order, windowType);

            //Make sure the window is symmetric
            for (int i = 0; i < window.Length; i++)
            {
                Debug.Assert(window[i] == window[window.Length - 1 - i]);
            }

            //Create filter and return
            return CreateFirFilter(freq, order, window, lowpass);
        }


        /// <summary>
        /// Creates a nth-order low-pass or high-pass Type I FIR filter with a specified window.
        /// </summary>
        /// <param name="freq">Corner frequency in radians.</param>
        /// <param name="order">Filter order.</param>
        /// <param name="window">Window type.</param>
        /// <param name="lowpass">True specifies low-pass, false specifies high-pass.</param>
        /// <returns></returns>
        public static FirFilter CreateFirFilter(double freq, int order, double[] window, bool lowpass)
        {

            if ((order % 2 == 1) && !lowpass)
                throw new ArgumentException("Cannot make a Type I highpass filter with an odd filter order");

            int filterLength = order+1;
            if(window.Length != filterLength)
                throw new ArgumentException("Window must have a length of order + 1");

            
            
            //Create ideal impulse response
            double[] idealImpulseResponse = new double[filterLength];
            if (lowpass)
            {
                for (int i = 0; i < idealImpulseResponse.Length; i++)
                {
                    idealImpulseResponse[i] = Sinc(freq * (i - filterLength / 2)) * freq / Math.PI;
                }
            }
            else
            {
                for (int i = 0; i < idealImpulseResponse.Length; i++)
                {
                    idealImpulseResponse[i] = Sinc(Math.PI * (i-filterLength/2)) - Sinc(freq * (i - filterLength / 2)) * freq / Math.PI;
                }
            }

            //Make sure the ideal impulse response is symmetric
            for (int i = 0; i < idealImpulseResponse.Length; i++)
            {
                Debug.Assert(idealImpulseResponse[i] == idealImpulseResponse[idealImpulseResponse.Length - 1 - i]);
            }


            //Window ideal impulse response
            double[] impulseResponse = (double[])idealImpulseResponse.Zip(window, (x, y) => x * y);

            //Scale filter to have pass-band approximately equal to one
            double[] scaledImpulseResponse;
            if (lowpass)
            {
                //Scale everything so we have unity gain at the DC offset (0 Hz)
                double dcValue = impulseResponse.Sum();
                scaledImpulseResponse = Array.ConvertAll(impulseResponse, x => x / dcValue);
            }
            else
            {
                //Scale everything so we have unity gain at Nyquist frequency (Pi Hz)
                double f0 = Math.PI;
                Complex[] freqs = new Complex[filterLength];
                for(int i=0; i<freqs.Length; i++)
                {
                    freqs[i] = Complex.FromPolar(impulseResponse[i], -Math.PI*i*f0);
                }

                double nyquistValue = freqs.Aggregate((x,y) => x + y).Magnitude;

                scaledImpulseResponse = Array.ConvertAll(impulseResponse, x => x / nyquistValue);
            }


            //Make sure the ideal impulse response is symmetric
            for (int i = 0; i < idealImpulseResponse.Length; i++)
            {
                Debug.Assert(idealImpulseResponse[i] == idealImpulseResponse[idealImpulseResponse.Length - 1 - i]);
            }


            return new FirFilter(new TransferFunction(scaledImpulseResponse, new[] { 1.0 }));
        }

        /// <summary>
        /// Computes the sinc function.  sin(angle) / angle
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        static double Sinc(double angle)
        {
            if (angle == 0)
                return 1;

            return Math.Sin(angle) / angle;
        }

        #endregion



        #region Private Methods

        static IirFilter AnalogPrototypeToDigitalFilter(ZeroPoleGain analogPrototype, Pair<double,double> cornerFreqs, BandType bandType, IirFilterType filterType)
        {

            // Check that we have valid frequencies
            bool isError = false;
            isError |= (cornerFreqs.First < 0 || cornerFreqs.First > Math.PI);

            if(bandType == BandType.BandPass || bandType == BandType.BandStop)
            {
                isError |= (cornerFreqs.Second < 0 || cornerFreqs.Second > Math.PI);
            }

            if (isError)
                throw new Exception("Frequencies must be between 0 and PI inclusive");
            


            //Prewarp the corner frequencies
            //Assuming sampling frequency of 1/2 so that we don't need to account for sampling frequency when converting from analog to digital.
            const double samplingFreq = 0.5;
            Func<double, double> freqWarp = x => 2 * samplingFreq * Math.Tan(x/2);
            Pair<double, double> warpedCornerFreqs = new Pair<double, double>();
            warpedCornerFreqs.First = freqWarp(cornerFreqs.First);
            warpedCornerFreqs.Second = freqWarp(cornerFreqs.Second);


            // Set prototype to desired band
            ZeroPoleGain analogFilter;
            switch (bandType)
            {
                case BandType.LowPass:
                    analogFilter = FilterModifiers.AnalogPrototypeToLowPass(warpedCornerFreqs.First, analogPrototype);
                    break;

                case BandType.HighPass:
                    analogFilter = FilterModifiers.AnalogPrototypeToHighPass(warpedCornerFreqs.First, analogPrototype);
                    break;

                case BandType.BandPass:
                    analogFilter = FilterModifiers.AnalogPrototypeToBandPass(warpedCornerFreqs.First, warpedCornerFreqs.Second, analogPrototype);
                    break;

                case BandType.BandStop:
                    analogFilter = FilterModifiers.AnalogPrototypeToBandStop(warpedCornerFreqs.First, warpedCornerFreqs.Second, analogPrototype);
                    break;

                default: Debug.Assert(false); throw new Exception();
            }
            
            //Convert analog (continuous) filter to digital (discrete) filter
            ZeroPoleGain digitalFilter;
            digitalFilter = FilterModifiers.ConvertAnalogToDigital(analogFilter);


            //Convert digital filter into second-order sections
            SosGain sosGain = FilterModifiers.ConvertZeroPoleToSosFilter(digitalFilter);


            //Create filter and return it
            return new IirFilter(sosGain);
        }



        #endregion


    }
}
