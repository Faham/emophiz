using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


using SensorLib.Util;

namespace SensorLib.Filters.FilterCreation
{
    public static class OrderSelectors
    {

        public static FilterOrderSpec ConvertFrequencySpecToOrderSpec(FilterFrequencySpec freqSpec)
        {

            // Do sanity checks on the frequency spec
            bool singleTransition = (freqSpec.BandType == BandType.LowPass || freqSpec.BandType == BandType.HighPass);

            if (freqSpec.BandType == BandType.LowPass && freqSpec.PassFreqs.First >= freqSpec.StopFreqs.First)
                throw new Exception("A low-pass filter must have its pass-band frequency less than its stop-band frequency.");

            bool isError = false;

            isError |= (freqSpec.StopFreqs.First < 0 && freqSpec.StopFreqs.First > Math.PI);
            isError |= (freqSpec.StopFreqs.Second < 0 && freqSpec.StopFreqs.Second > Math.PI);

            isError |= (freqSpec.StopFreqs.First < 0 && freqSpec.StopFreqs.First > Math.PI);
            isError |= (freqSpec.StopFreqs.Second < 0 && freqSpec.StopFreqs.Second > Math.PI);

            if (isError)
                throw new Exception("Frequencies must be between 0 and PI inclusive");

            //!! Make sure we did all checks


            switch (freqSpec.FilterType)
            {
                case IirFilterType.Butterworth: return Butterworth(freqSpec);
                case IirFilterType.ChebyshevType1: return ChebyshevType1(freqSpec);
                case IirFilterType.ChebyshevType2: return ChebyshevType2(freqSpec);
                default: Debug.Assert(false); throw new Exception();
            }
        }


        #region Freq-Spec to Order-Spec Conversion Methods

        /// <summary>
        /// Gets the minimum filter order and corner frequency needed to meet the design specifications of a low-pass or high-pass filter.
        /// All parameters are in the continuous (analog) domain.  Based off of Matlab's buttord function.
        /// </summary>
        static FilterOrderSpec Butterworth(FilterFrequencySpec freqSpec)
        {
            Debug.Assert(freqSpec.FilterType == IirFilterType.Butterworth);

            bool singleTransition = (freqSpec.BandType == BandType.LowPass || freqSpec.BandType == BandType.HighPass);

            // Get stop frequency if filter were an analog prototype (low-pass with pass frequency at 1)
            double prototypeStopFreq;
            if (singleTransition)
                prototypeStopFreq = FindPrototypeStopFreq(freqSpec.PassFreqs.First, freqSpec.StopFreqs.First);
            else
                prototypeStopFreq = FindPrototypeStopFreq(freqSpec.PassFreqs, freqSpec.StopFreqs);

            

            // Find the minimum order needed to meet the spec
            double stopRipplePart = Math.Pow(10, 0.1 * Math.Abs(freqSpec.StopRipple)) - 1;
            double passRipplePart = Math.Pow(10, 0.1 * Math.Abs(freqSpec.PassRipple)) - 1;
            int order = (int)Math.Ceiling(Math.Log10(stopRipplePart / passRipplePart) / (2 * Math.Log10(prototypeStopFreq)));

            if (order == 0)
                order = 1;

            // Find the corner frequency which will give exactly stopRipple dB at the prototype stop frequency.
            double prototypeCornerFreq = prototypeStopFreq / Math.Pow(stopRipplePart, 1 / (2 * Math.Abs(order)));


            // Convert this frequency back to original analog filter type
            Pair<double, double> cornerFreqs = new Pair<double, double>(0.0, 0.0);
            switch (freqSpec.BandType)
            {
                case BandType.LowPass:
                    cornerFreqs.First = prototypeCornerFreq * freqSpec.PassFreqs.First;
                    break;

                case BandType.HighPass:
                    cornerFreqs.First = freqSpec.PassFreqs.First / prototypeCornerFreq;
                    break;

                case BandType.BandPass:
                    double passFreq1 = freqSpec.PassFreqs.First;
                    double passFreq2 = freqSpec.PassFreqs.Second;
                    double bandWidth = passFreq2 - passFreq1;
                    Func<double, double> transform = x => -x * bandWidth / 2 + Math.Sqrt(x * x / (4*bandWidth*bandWidth + passFreq1*passFreq2));
                    double firstCornerFreq = transform(prototypeCornerFreq);
                    double secondCornerFreq = transform(-prototypeCornerFreq);
                    cornerFreqs.First = Math.Max(firstCornerFreq, secondCornerFreq);
                    cornerFreqs.Second = Math.Min(firstCornerFreq, secondCornerFreq);
                    break;

                case BandType.BandStop:
                    throw new NotSupportedException("Creating a band-stop Butterworth filter from a frequency spec is not supported");

                default: Debug.Assert(false); throw new Exception();
            }

            // Create the filter order spec
            return FilterOrderSpec.CreateButterworthSpec(cornerFreqs, order, freqSpec.BandType);
        }



        /// <summary>
        /// Gets the minimum filter order and corner frequency needed to meet the design specifications of a low-pass or high-pass filter.
        /// All parameters are in the continuous (analog) domain.  Based off of Matlab's cheb1ord function.
        /// </summary>
        static FilterOrderSpec ChebyshevType1(FilterFrequencySpec freqSpec)
        {

            Debug.Assert(freqSpec.FilterType == IirFilterType.ChebyshevType1);

            bool singleTransition = (freqSpec.BandType == BandType.LowPass || freqSpec.BandType == BandType.HighPass);

            // Get stop frequency if filter were an analog prototype (low-pass with pass frequency at 1)
            double prototypeStopFreq;
            if (singleTransition)
                prototypeStopFreq = FindPrototypeStopFreq(freqSpec.PassFreqs.First, freqSpec.StopFreqs.First);
            else
                prototypeStopFreq = FindPrototypeStopFreq(freqSpec.PassFreqs, freqSpec.StopFreqs);


            //Find the minimum order needed to meet the spec
            double stopRipplePart = Math.Pow(10, 0.1 * Math.Abs(freqSpec.StopRipple)) - 1;
            double passRipplePart = Math.Pow(10, 0.1 * Math.Abs(freqSpec.PassRipple)) - 1;
            int order = (int)Math.Ceiling(SpecialMath.acosh(Math.Sqrt(stopRipplePart / passRipplePart)) / SpecialMath.acosh(prototypeStopFreq));

            // Convert this frequency back to original analog filter type
            Pair<double, double> cornerFreqs = new Pair<double, double>(0.0, 0.0);
            
            // Natural frequencies are just the passband frequencies
            cornerFreqs.First = freqSpec.PassFreqs.First;
            cornerFreqs.Second = freqSpec.PassFreqs.Second;

            // Create the filter order spec
            return FilterOrderSpec.CreateChebyshevType1Spec(cornerFreqs, order, freqSpec.PassRipple, freqSpec.BandType);
        }



        /// <summary>
        /// Gets the minimum filter order and corner frequency needed to meet the design specifications of a low-pass or high-pass filter.
        /// All parameters are in the continuous (analog) domain.  Based off of Matlab's cheb2ord function.
        /// </summary>
        static FilterOrderSpec ChebyshevType2(FilterFrequencySpec freqSpec)
        {

            Debug.Assert(freqSpec.FilterType == IirFilterType.ChebyshevType2);

            bool singleTransition = (freqSpec.BandType == BandType.LowPass || freqSpec.BandType == BandType.HighPass);

            // Get stop frequency if filter were an analog prototype (low-pass with pass frequency at 1)
            double prototypeStopFreq;
            if (singleTransition)
                prototypeStopFreq = FindPrototypeStopFreq(freqSpec.PassFreqs.First, freqSpec.StopFreqs.First);
            else
                prototypeStopFreq = FindPrototypeStopFreq(freqSpec.PassFreqs, freqSpec.StopFreqs);


            //Find the minimum order needed to meet the spec
            double stopRipplePart = Math.Pow(10, 0.1 * Math.Abs(freqSpec.StopRipple)) - 1;
            double passRipplePart = Math.Pow(10, 0.1 * Math.Abs(freqSpec.PassRipple)) - 1;
            int order = (int)Math.Ceiling(SpecialMath.acosh(Math.Sqrt(stopRipplePart / passRipplePart)) / SpecialMath.acosh(prototypeStopFreq));

            //Find the corner frequency which will give exactly stopRipple dB at the prototype stop frequency.
            double prototypeCornerFreq = 1/Math.Cosh(SpecialMath.acosh(Math.Sqrt(stopRipplePart / passRipplePart) / order));

            // Convert this frequency back to original analog filter type
            Pair<double, double> cornerFreqs = new Pair<double, double>(0.0, 0.0);
            switch (freqSpec.BandType)
            {
                case BandType.LowPass:
                    cornerFreqs.First = freqSpec.PassFreqs.First / prototypeCornerFreq;
                    break;

                case BandType.HighPass:
                    cornerFreqs.First = freqSpec.PassFreqs.First * prototypeCornerFreq;
                    break;

                case BandType.BandPass:
                    double x = prototypeCornerFreq;
                    double passFreq1 = freqSpec.PassFreqs.First;
                    double passFreq2 = freqSpec.PassFreqs.Second;
                    double bandWidth = passFreq2 - passFreq1;
                    cornerFreqs.First = (-bandWidth) / (2 * x) + Math.Sqrt(bandWidth*bandWidth / (4 * x * x) + passFreq1 * passFreq2);
                    cornerFreqs.Second = passFreq1 * passFreq2 / cornerFreqs.First;
                    break;

                case BandType.BandStop:
                    throw new NotSupportedException("Creating a band-stop Chebyshev Type II filter from a frequency spec is not supported");

                default: Debug.Assert(false); throw new Exception();
            }

            // Create the filter order spec
            return FilterOrderSpec.CreateChebyshevType2Spec(cornerFreqs, order, freqSpec.StopRipple, freqSpec.BandType);
        }



        #endregion




        static double FindPrototypeStopFreq(double passFreq, double stopFreq)
        {
            Debug.Assert(passFreq != stopFreq);

            bool isLowPass = passFreq < stopFreq;

            //Get stop frequency if filter were an analog prototype (low-pass with pass frequency at 1)
            double prototypeStopFreq;
            if (isLowPass)
                prototypeStopFreq = stopFreq / passFreq;
            else
                prototypeStopFreq = passFreq / stopFreq;

            return prototypeStopFreq;
        }


        static double FindPrototypeStopFreq(Pair<double,double> passFreq, Pair<double,double> stopFreq)
        {
            bool isBandPass = passFreq.First > stopFreq.First;


            //Get stop frequency if filter were an analog prototype (low-pass with pass frequency at 1)
            double prototypeStopFreq1;
            double prototypeStopFreq2;
            if (isBandPass)
            {
                Debug.Assert(stopFreq.First < passFreq.First);
                Debug.Assert(passFreq.First < passFreq.Second);
                Debug.Assert(passFreq.Second < stopFreq.Second);

                prototypeStopFreq1 = (stopFreq.First * stopFreq.First - passFreq.First * passFreq.Second) / (stopFreq.First * (passFreq.First - passFreq.Second));
                prototypeStopFreq2 = (stopFreq.Second * stopFreq.Second - passFreq.First * passFreq.Second) / (stopFreq.Second * (passFreq.First - passFreq.Second));
            }
            else
            {
                Debug.Assert(passFreq.First < stopFreq.First);
                Debug.Assert(stopFreq.First < stopFreq.Second);
                Debug.Assert(stopFreq.Second < passFreq.Second);

                throw new NotImplementedException();
            }

            //Find the more demanding spec we need to meet
            double prototypeStopFreq = Math.Min(Math.Abs(prototypeStopFreq1), Math.Abs(prototypeStopFreq2));

            return prototypeStopFreq;
        }


    }
}
