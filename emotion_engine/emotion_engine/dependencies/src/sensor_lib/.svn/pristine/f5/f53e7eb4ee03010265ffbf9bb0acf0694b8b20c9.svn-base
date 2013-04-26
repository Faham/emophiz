using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


using SensorLib.Filters.RealTime;
using SensorLib.Util;

namespace SensorLib.Filters.FilterCreation
{
    class FilterModifiers
    {



        /// <summary>
        /// Converts an analog prototype filter into a low-pass filter with a desired corner frequency.  Similiar to lp2lp function in Matlab.
        /// Based off of http://www.mikroe.com/eng/chapters/view/73/chapter-3-iir-filters/.
        /// Uses the transform s -> wc*s where wc is the new corner frequency.
        /// </summary>
        /// <param name="cornerFreq">The corner frequency of the filter in radians.</param>
        /// <param name="prototype">The analog prototype filter.</param>
        public static ZeroPoleGain AnalogPrototypeToLowPass(double cornerFreq, ZeroPoleGain prototype)
        {


            //Transformation for zeros and poles
            Converter<Complex, Complex> zeroPoleTransform = x => cornerFreq * x;

            //Transform the zeros and poles
            Complex[] zeros = Array.ConvertAll(prototype.Zeros, zeroPoleTransform);
            Complex[] poles = Array.ConvertAll(prototype.Poles, zeroPoleTransform);

            //Calculate the gain
            int zeroCount = prototype.Zeros.Length;
            int poleCount = prototype.Poles.Length;
            double gain = prototype.Gain * Math.Pow(cornerFreq, poleCount-zeroCount);

            return new ZeroPoleGain(gain, zeros, poles);
        }


        /// <summary>
        /// Converts an analog prototype filter into a high-pass filter with a desired corner frequency.  Similiar to lp2hp function in Matlab.
        /// Based off of http://www.mikroe.com/eng/chapters/view/73/chapter-3-iir-filters/.
        /// Uses the transform s -> wc/s where wc is the new corner frequency.
        /// </summary>
        /// <param name="cornerFreq">The corner frequency of the filter in radians.</param>
        /// <param name="prototype">The analog prototype filter.</param>
        public static ZeroPoleGain AnalogPrototypeToHighPass(double cornerFreq, ZeroPoleGain prototype)
        {

            //Transformation for zeros and poles
            Converter<Complex, Complex> zeroPoleTransform = x => cornerFreq / x;

            //Transform the zeros and poles
            Complex[] zeros = Array.ConvertAll(prototype.Zeros, zeroPoleTransform);
            Complex[] poles = Array.ConvertAll(prototype.Poles, zeroPoleTransform);

            //Calculate the gain
            Complex zeroMult = prototype.Zeros.Aggregate(Complex.One, (x, y) => x * -y);
            Complex poleMult = prototype.Poles.Aggregate(Complex.One, (x, y) => x * -y);
            double gain = (prototype.Gain * zeroMult / poleMult).Real;

            //Add zeros
            int zeroCount = prototype.Zeros.Length;
            int poleCount = prototype.Poles.Length;
            zeros = zeros.Concat(Enumerable.Repeat(Complex.Zero, poleCount - zeroCount)).ToArray();

            return new ZeroPoleGain(gain, zeros, poles);
        }

        
        /// <summary>
        /// Converts an analog prototype filter into a band-pass filter with a desired frequency.  Similiar to lp2bp function in Matlab.
        /// Based off of http://www.mikroe.com/eng/chapters/view/73/chapter-3-iir-filters/.
        /// Uses the transform s -> (s^2 + wp1*wp2) / (s(wp2 - wp1)) where wp1 and wp2 are the new corner frequencies.
        /// </summary>
        /// <param name="freq1">The left corner frequency of the filter in radians.</param>
        /// <param name="freq2">The right corner frequency of the filter in radians.</param>
        /// <param name="prototype">The analog prototype filter.</param>
        public static ZeroPoleGain AnalogPrototypeToBandPass(double freq1, double freq2, ZeroPoleGain prototype)
        {

            //Get the bandwidth
            double bw = freq2 - freq1;


            //Transformation for zeros and poles
            Converter<Complex, Complex> zeroPoleTransform = x => ((bw*x) + ((bw*bw*x*x) - 4*freq1*freq2).SquareRoot)/2;
            Converter<Complex, Complex> zeroPoleTransform2 = x => ((bw*x) - ((bw*bw*x*x) - 4*freq1*freq2).SquareRoot)/2;

            //Transform the zeros and poles
            Complex[] zeros = Array.ConvertAll(prototype.Zeros, zeroPoleTransform);
            zeros = zeros.Concat(Array.ConvertAll(prototype.Zeros, zeroPoleTransform2)).ToArray();

            Complex[] poles = Array.ConvertAll(prototype.Poles, zeroPoleTransform);
            poles = poles.Concat(Array.ConvertAll(prototype.Poles, zeroPoleTransform2)).ToArray();

            //Calculate the gain
            int zeroCount = prototype.Zeros.Length;
            int poleCount = prototype.Poles.Length;
            double gain = prototype.Gain * Math.Pow(bw, poleCount - zeroCount);

            //Add zeros
            zeros = zeros.Concat(Enumerable.Repeat(Complex.Zero, poleCount - zeroCount)).ToArray();

            return new ZeroPoleGain(gain, zeros, poles);
        }


        /// <summary>
        /// Converts an analog prototype filter into a band-stop filter with a desired frequency.  Similiar to lp2bs function in Matlab.
        /// Based off of http://www.mikroe.com/eng/chapters/view/73/chapter-3-iir-filters/.
        /// Uses the transform s -> s(ws2 - ws1) / (s^2 + ws1*ws2) where ws1 and ws2 are the new corner frequencies.
        /// </summary>
        /// <param name="freq1">The left corner frequency of the filter in radians.</param>
        /// <param name="freq2">The right corner frequency of the filter in radians.</param>
        /// <param name="prototype">The analog prototype filter.</param>
        public static ZeroPoleGain AnalogPrototypeToBandStop(double freq1, double freq2, ZeroPoleGain prototype)
        {

            // Get the bandwidth
            double bw = freq2 - freq1;

            // Transformation for zeros and poles
            Converter<Complex, Complex> zeroPoleTransform = x => (bw + (bw * bw - 4 * freq1 * freq2 * x * x).SquareRoot) / (2 * x);
            Converter<Complex, Complex> zeroPoleTransform2 = x => (bw - (bw * bw - 4 * freq1 * freq2 * x * x).SquareRoot) / (2 * x);

            // Transform the zeros and poles
            Complex[] zeros = Array.ConvertAll(prototype.Zeros, zeroPoleTransform);
            zeros = zeros.Concat(Array.ConvertAll(prototype.Zeros, zeroPoleTransform2)).ToArray();

            Complex[] poles = Array.ConvertAll(prototype.Poles, zeroPoleTransform);
            poles = poles.Concat(Array.ConvertAll(prototype.Poles, zeroPoleTransform2)).ToArray();

            // Calculate the gain
            Complex zeroMult = prototype.Zeros.Aggregate(Complex.One, (x, y) => x * -y);
            Complex poleMult = prototype.Poles.Aggregate(Complex.One, (x, y) => x * -y);
            double gain = (prototype.Gain * zeroMult / poleMult).Real;  //Not sure why this isn't "gain = protoType.gain"

            // Add zeros and poles
            int zeroCount = prototype.Zeros.Length;
            int poleCount = prototype.Poles.Length;
            {
                // Get position of new zeros and poles
                Complex newZeroPoles = Complex.Eye * Math.Sqrt(freq1 * freq2);

                // Get number of zeros to add
                int zeroAddCount = poleCount - zeroCount;
                if (zeroAddCount < 0)
                    zeroAddCount = 0;

                // Add zeros
                IEnumerable<Complex> newZeros = Enumerable.Repeat(newZeroPoles, zeroAddCount).Concat(Enumerable.Repeat(-newZeroPoles, zeroAddCount));

                // Get number of poles to add
                int poleAddCount = zeroCount - poleCount;
                if (poleAddCount < 0)
                    poleAddCount = 0;

                // Add poles
                IEnumerable<Complex> newPoles = Enumerable.Repeat(newZeroPoles, poleAddCount).Concat(Enumerable.Repeat(-newZeroPoles, poleAddCount));


                zeros = zeros.Concat(newZeros).ToArray();
                poles = poles.Concat(newPoles).ToArray();
            }

            return new ZeroPoleGain(gain, zeros, poles);
        }



        /// <summary>
        /// Converts the continuous analog filter to its discrete digital equivalent.  Similiar to bilinear function in Matlab.
        /// Based off of http://www.mikroe.com/eng/chapters/view/73/chapter-3-iir-filters/.
        /// Uses the transform s = (z-1) / (z+1)
        /// </summary>
        /// <param name="analog">The analog filter in zero-pole-gain form.</param>
        /// <param name="sampleRate">The sample rate of the filter in samples/second.</param>
        public static ZeroPoleGain ConvertAnalogToDigital(ZeroPoleGain analog)
        {

            //The numerator order (number of zeros) cannot be higher than the denominator order (number of poles)
            if (analog.Zeros.Length > analog.Poles.Length)
                throw new Exception("The numerator order (number of zeros) cannot be higher than the denominator order (number of poles)");


            //Get the order of the discrete filter
            int filterOrder = analog.Poles.Length;


            //Prewarp
            //Don't need to do this because we warp the corner frequency earlier
            

            //Remove all zeros at infinite
            //Don't need to do this because we shouldn't have any zeros at infinite.



            //Transformation for zeros and poles
            Converter<Complex, Complex> zeroPoleTransform = x => (1 + x) / (1 - x);


            //Transform the zeros and poles
            Complex[] zeros = Array.ConvertAll(analog.Zeros, zeroPoleTransform);
            Complex[] poles = Array.ConvertAll(analog.Poles, zeroPoleTransform);

            //Calculate the gain
            int zeroCount = analog.Zeros.Length;
            int poleCount = analog.Poles.Length;
            Complex zeroMult = analog.Zeros.Aggregate(Complex.One, (x, y) => x * (1 - y));
            Complex poleMult = analog.Poles.Aggregate(Complex.One, (x, y) => x * (1 - y));
            double gain = (analog.Gain * zeroMult / poleMult).Real;

            //Add zeros
            zeros = zeros.Concat(Enumerable.Repeat(-Complex.One, poleCount-zeroCount)).ToArray();

            return new ZeroPoleGain(gain, zeros, poles);
        }


        
        //!! Is there a more straightforward way to do this?
        public static TransferFunction ConvertZeroPoleToTransferFunction(ZeroPoleGain zeroPoleGain)
        {
            SosGain sosGain = ConvertZeroPoleToSosFilter(zeroPoleGain);
            return ConvertSosToTransferFunction(sosGain);
        }


        // Calculate transfer function polynomials of the sections together (similiar to sos2tf in matlab)
        public static TransferFunction ConvertSosToTransferFunction(SosGain sosGain)
        {
            double[] coefA = sosGain.Sections.Select(x => x.TransferFunction.A).Aggregate(new[] { 1.0 }, (x, y) => Helpers.Convolution(x, y));
            double[] coefB = sosGain.Sections.Select(x => x.TransferFunction.B).Aggregate(new[] { 1.0 }, (x, y) => Helpers.Convolution(x, y));

            // Incorporate gain into transfer function
            coefB = Array.ConvertAll(coefB, x => sosGain.Gain * x);

            // Remove trailing zeros
            if (coefA.Length > 3 && coefA.Last() == 0)
                Array.Resize(ref coefA, coefA.Length - 1);

            if (coefB.Length > 3 && coefB.Last() == 0)
                Array.Resize(ref coefB, coefB.Length - 1);

            return new TransferFunction(coefB, coefA);
        }


        public static ZeroPoleGain ConvertSosToZeroPole(SosGain sosGain)
        {
            double gain = sosGain.Gain;
            Complex[] zeros = sosGain.Sections.SelectMany(x => x.ZeroPoleGain.Zeros).ToArray();
            Complex[] poles = sosGain.Sections.SelectMany(x => x.ZeroPoleGain.Poles).ToArray();

            return new ZeroPoleGain(gain, zeros, poles);
        }


        /// <summary>
        /// Converts the zero-pole-gain filter into a filter with second-order sections.  Similiar to zp2sos function in Matlab.
        /// Based off of the Matlab function.
        /// </summary>
        /// <param name="filter">The filter in zero-pole-gain format.</param>
        /// <returns>The filter as second-order sections.</returns>
        public static SosGain ConvertZeroPoleToSosFilter(ZeroPoleGain zeroPoleGain)
        {
            // Make sure there are an equal number of poles and zeros
            if (zeroPoleGain.Zeros.Length != zeroPoleGain.Poles.Length)
                throw new Exception("There must be an equal number of poles and zeros.");


            // Sort the zeros and poles
            Func<Complex, bool> posImaginary = x => (!x.IsRealWithTolerance) && (x.Imaginary > 0);
            Func<Complex, bool> negImaginary = x => (!x.IsRealWithTolerance) && (x.Imaginary < 0);
            Func<Complex, bool> real = x => x.IsRealWithTolerance;


            // Group the zeros into where they are on the plot
            List<Complex> zerosPosImaginary = zeroPoleGain.Zeros.Where(posImaginary).ToList();
            List<Complex> zerosNegImaginary = zeroPoleGain.Zeros.Where(negImaginary).ToList();
            List<Complex> zerosReal = zeroPoleGain.Zeros.Where(real).ToList();

            // Group the poles into where they are on the plot
            List<Complex> polesPosImaginary = zeroPoleGain.Poles.Where(posImaginary).ToList();
            List<Complex> polesNegImaginary = zeroPoleGain.Poles.Where(negImaginary).ToList();
            List<Complex> polesReal = zeroPoleGain.Poles.Where(real).ToList();

            //!! Make sure zeros and poles have all their pairs!
            
            // Create second-order sections based on the following rules (similar to matlab:zp2sos):
            // 1. Match poles closest to the unit circle with zeros closest to those poles.
            // 2. Match the poles next closest to the unit circle with the zeros closest to those poles.
            // 3. Continue until all of the poles and zeros are mtached.
            // 4. Group real poles into sections with other real poles closest to them in absolute value (same for real zeros).
            List<Sos> sosList = new List<Sos>();
            Complex[] sortedPoles = polesPosImaginary.OrderByDescending(x => x.Magnitude).Concat(polesReal.OrderByDescending(x => x.Magnitude)).ToArray();
            List<Complex> zeros = zerosPosImaginary.Concat(zerosReal).ToList();
            int poleCount = 0;
            int orderCount = 0;
            while(poleCount < sortedPoles.Length)
            {
                
                List<Complex> sosPoles = new List<Complex>();

                // Add first pole
                sosPoles.Add(sortedPoles[poleCount]);
                poleCount++;

                orderCount++;

                // Add second pole if there is one
                if (orderCount < zeroPoleGain.Poles.Length)
                {
                    switch (sosPoles[0].IsRealWithTolerance)
                    {
                        case false:
                            // Add complex conjugate
                            sosPoles.Add(sosPoles[0].Conjugate);
                            break;

                        case true:
                            // Add another real
                            sosPoles.Add(sortedPoles[poleCount]);
                            poleCount++;
                            break;

                        default:
                            Debug.Assert(false); throw new Exception();
                    }

                    orderCount++;
                }


                List<Complex> sosZeros = new List<Complex>();
                Func<IEnumerable<Complex>, Func<Complex,double>, Complex> Max = (x,y) => x.Aggregate((w, z) => y(w) >= y(z) ? w : z);
                Func<IEnumerable<Complex>, Func<Complex, double>, Complex> Min = (x, y) => x.Aggregate((w, z) => y(w) <= y(z) ? w : z);
                
                // Add zero closest to first pole and remove it
                Debug.Assert(zeros.Count > 0);
                sosZeros.Add(Min(zeros, x => (sosPoles[0] - x).Magnitude));
                zeros.Remove(sosZeros[0]);

                if (sosPoles.Count == 1)
                {
                    Debug.Assert(sosZeros[0].IsRealWithTolerance);
                    Debug.Assert(zeros.Count == 0);
                }

                // Add second zero and remove it
                if (sosPoles.Count == 2)
                {
                    switch (sosZeros[0].IsRealWithTolerance)
                    {
                        case false:
                            // Add complex conjugate
                            sosZeros.Add(sosZeros[0].Conjugate);
                            break;

                        case true:
                            // Add zero closest to the first zero
                            sosZeros.Add(Min(zeros, x => (sosZeros[0] - x).Magnitude));
                            
                            // Remove the zero
                            zeros.Remove(sosZeros[1]);
                            break;

                        default:
                            Debug.Assert(false); throw new Exception();
                    }

                }


                //Create a second-order section
                sosList.Add(new Sos(new ZeroPoleGain(1, sosZeros.ToArray(), sosPoles.ToArray())));
            }

            //Reverse sos sections so that the sections with the pole closest to the unit circle are at the end
            sosList.Reverse();

            // Return sos-gain form
            return new SosGain(zeroPoleGain.Gain, sosList.ToArray());
        }

        

        /// <summary>
        /// Will group a list of complex numbers into conjugate pairs and reals.  Similiar to cplxpair function in Matlab.
        /// </summary>
        /// <param name="roots"></param>
        /// <param name="pairs"></param>
        /// <param name="reals"></param>
        static void Group(List<Complex> roots, out List<Complex> pairs, out List<Complex> reals)
        {
            //Sort the zeros and poles
            Func<Complex, bool> posImaginaryTest = x => (!x.IsRealWithTolerance) && (x.Imaginary > 0);
            Func<Complex, bool> negImaginaryTest = x => (!x.IsRealWithTolerance) && (x.Imaginary < 0);
            Func<Complex, bool> realTest = x => x.IsRealWithTolerance;

            //Group the roots into where they are on the plot
            List<Complex> posImaginary = roots.Where(posImaginaryTest).ToList();
            List<Complex> negImaginary = roots.Where(negImaginaryTest).ToList();
            reals = roots.Where(realTest).ToList();

            if(posImaginary.Count != negImaginary.Count)
                throw new Exception("Could not pair up conjugates.");

            //Pair up the complex conjugates
            pairs = new List<Complex>();
            foreach (Complex num in posImaginary)
            {
                //Find conjugate pair and remove it
                Predicate<Complex> isConjugate = x => (num + x).IsRealWithTolerance;
                int index = negImaginary.FindIndex(isConjugate);
                Complex conjugate = negImaginary[index];
                negImaginary.RemoveAt(index);

                pairs.Add(conjugate);
            }

            if(negImaginary.Count != 0)
                throw new Exception("Could not pair up conjugates.");


            Debug.Assert(pairs.Count + reals.Count == roots.Count);
            return;
        }


        


        #region Obselete

        /*
        /// <summary>
        /// Represents a filter in state-space form.  (dx = Ax + Bu, y = Cx + Du)
        /// </summary>
        public struct StateSpace
        {
            public Complex A { get; set; }
            public Complex B { get; set; }
            public Complex C { get; set; }
            public Complex D { get; set; }
        }

        
        /// <summary>
        /// Converts a filter from zero-pole-gain form to state-space form.  Similiar to zp2ss function in Matlab.
        /// Only does single-input, single-output.
        /// </summary>
        /// <param name="zpg"></param>
        /// <param name="ss"></param>
        public static void ZeroPoleGainToStateSpace(ZeroPoleGain zpg, out StateSpace ss)
        {
            //Strip infinities and throw away
            //!!

            //Allocate our new state-space representation
            ss = new StateSpace();

            //Get number of poles and zeros
            int zeroNum = zpg.Zeros.Length;
            int poleNum = zpg.Poles.Length;

            //Find if there is an odd number of zeros and poles
            bool oddZeros = (zpg.Zeros.Length % 2) != 0;
            bool oddPoles = (zpg.Poles.Length % 2) != 0;


            //Group the zeros into their conjugate pairs
            List<Complex> zeroPairs;
            List<Complex> zeroReals;
            Group(zpg.Zeros.ToList(), out zeroPairs, out zeroReals);
            List<Complex> zeros = new List<Complex>();
            zeros.AddRange(zeroPairs);
            zeros.AddRange(zeroReals);

            //Group the zeros into their conjugate pairs
            List<Complex> polePairs;
            List<Complex> poleReals;
            Group(zpg.Poles.ToList(), out polePairs, out poleReals);
            List<Complex> poles = new List<Complex>();
            poles.AddRange(polePairs);
            poles.AddRange(poleReals);

            
            //If odd number of poles and zeros, convert the pole and zero at the end into state-space
            //H(s) = (s-z1)/(s-p1) = (s + num(2)) / (s + den(2))
            if (oddZeros && oddPoles)
            {
                Complex zero = zeros.Last();
                zeros.RemoveAt(zeros.Count - 1);

                Complex pole = poles.Last();
                poles.RemoveAt(poles.Count - 1);

                ss.A = pole;
                ss.B = Complex.One;
                ss.C = pole - zero;
                ss.D = Complex.One;
                
            }
            else if (oddPoles)  //Convert the pole at the end into state-space.  H(s) = 1/(s-p1) = 1/(s+den(2))
            {
                Complex pole = poles.Last();
                poles.RemoveAt(poles.Count - 1);

                ss.A = pole;
                ss.B = Complex.One;
                ss.C = Complex.One;
                ss.D = Complex.Zero;

            }
            else if (oddZeros)  //Convert the zero at the end along with a pole-pair into state-space.  H(s) = (s+num(2))/(s^2 + den(2)*s + den(3))
            {
                //Get zero
                Complex zero = zeros.Last();
                zeros.RemoveAt(zeros.Count - 1);

                //Get poles
                Complex pole1 = poles.Last();
                poles.RemoveAt(poles.Count - 1);

                Complex pole2;
                if (pole1.IsRealWithTolerance)
                {
                    pole2 = poles.Last();
                    poles.RemoveAt(poles.Count - 1);
                }
                else
                {
                    pole2 = pole1.Conjugate;
                }


                //Create the numerator polynomial
                double[] numerator = new double[2];
                numerator[0] = 1;
                numerator[1] = -zero.Real;

                

                //Create the denominator polynomial
                double[] denominator = new double[3];
                denominator[0] = 1;
                denominator[1] = -(pole1 + pole2).Real;
                denominator[2] = (pole1*pole2).Real;


                double wn = Math.Sqrt(pole1.Magnitude * pole2.Magnitude);
                if (wn == 0)
                    wn = 1;

                /* //!!
                Matrix t = Matrix.Identity;
                ss.C = 
                ss.D = Complex.Zero;
                */
                /*
            }


            //Now we should have an even number of poles and zeros
            Debug.Assert((zeros.Count % 2) == 0);
            Debug.Assert((poles.Count % 2) == 0);


            throw new NotImplementedException();
        }
        */

        #endregion


    }
}
