using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Filters;
using SensorLib.Util;

namespace SensorLib.Filters.FilterCreation
{
    static class AnalogPrototypes
    {

        


        /// <summary>
        /// Gets the analog prototype of an n-order Butterworth filter (lowpass with wc = 1Hz).  Similiar to buttap function in Matlab.
        /// </summary>
        public static ZeroPoleGain Butterworth(int filterOrder)
        {
            if (filterOrder <= 0)
                throw new ArgumentException("Filter order must be greater than zero.");

            

            Complex[] zeros = new Complex[0];

            Complex complexGain = Complex.FromPolar(1, 0);
            Complex[] poles = new Complex[filterOrder];
            for (int i = 0; i < filterOrder; i++)
            {
                int index = i * 2 + 1;
                poles[i] = Complex.FromPolar(1, Math.PI * index / (2 * filterOrder) + (Math.PI / 2));
                complexGain *= -poles[i];
            }

            double gain = complexGain.Real;

            // Create zero-pole-gain form
            return new ZeroPoleGain(gain, zeros, poles);
        }


        /// <summary>
        /// Gets the analog prototype of an n-order Chebyshev Type I filter (lowpass with wc = 1Hz).  Similiar to cheb1ap function in Matlab.
        /// </summary>
        public static ZeroPoleGain ChebyshevType1(int filterOrder, double ripple)
        {
            if (filterOrder <= 0)
                throw new ArgumentException("Filter order must be greater than zero.");


            Complex[] zeros = new Complex[0];

            /*
            //double epsilon = Math.Sqrt(Math.Exp(.1*ripple)-1);
            //double mu = SpecialMath.asinh(1/epsilon) / filterOrder;

            Complex complexGain = Complex.FromPolar(1, 0);
            filter.Poles = new Complex[filterOrder];
            for (int i = 0; i < filterOrder; i++)
            {
                int index = i * 2 + 1;
                Complex pole = Complex.FromPolar(1, Math.PI*(index)/(2*filterOrder) + Math.PI/2);
                //realp = real(p);
                //double realp = (pole.Real + flipud(pole.Real)) /2;
                
                //imagp = imag(p);
                //imagp = (imagp - flipud(imagp))./2;
                p = new Complex(Math.Sinh(mu).*realp , Math.Cosh(mu).*imagp);



                filter.Poles[i] = pole;

                filter.Poles[i] = Complex.FromPolar(1, Math.PI * index / (2 * filterOrder) + (Math.PI / 2));
                complexGain *= -filter.Poles[i];
            }
            */


            double epsilon = Math.Sqrt(Math.Pow(10, .1 * ripple) - 1);
            double mu = SpecialMath.asinh(1 / epsilon) / filterOrder;


            Complex complexGain = Complex.FromPolar(1, 0);
            Complex[] poles = new Complex[filterOrder];
            for (int i = 0; i < filterOrder; i++)
            {
                int index = (i + 1) * 2 - 1;

                Complex pole = Complex.FromPolar(1, Math.PI * (index) / (2 * filterOrder) + Math.PI / 2);

                poles[i] = new Complex(Math.Sinh(mu) * pole.Real, Math.Cosh(mu) * pole.Imaginary);

                complexGain *= -poles[i];
            }


            double gain = complexGain.Real;

            //Matlab patches the gain if the filter order is even
            if (filterOrder % 2 == 0)
                gain = gain / Math.Sqrt(1 + epsilon * epsilon);

            // Create zero-pole-gain form
            return new ZeroPoleGain(gain, zeros, poles);
        }



        /// <summary>
        /// Gets the analog prototype of an n-order Chebyshev Type II filter (lowpass with wc = 1Hz).  Similiar to cheb2ap function in Matlab.
        /// </summary>
        public static ZeroPoleGain ChebyshevType2(int order, double ripple)
        {
            if (order <= 0)
                throw new ArgumentException("Filter order must be greater than zero.");


            double epsilon = 1 / Math.Sqrt(Math.Pow(10, .1 * ripple) - 1);
            double mu = SpecialMath.asinh(1 / epsilon) / order;


            //Calculate zeros
            Complex[] zeros;
            {
                IEnumerable<int> indices = Enumerable.Range(0, order/2).Select(i => (i + 1) * 2 - 1).Where(i => i != order);
                IEnumerable<Complex> zerosEnumerable = indices.Select(i => Complex.Eye / Math.Cos(Math.PI * i / (2 * order)));
                IEnumerable<Complex> conjugateZeros = zerosEnumerable.Select(x => x.Conjugate);
                zeros = zerosEnumerable.Concat(conjugateZeros).ToArray();
            }


            //Calculate poles
            Complex[] poles;
            {
                IEnumerable<int> indices = Enumerable.Range(0, order).Select(i => (i + 1) * 2 - 1);
                IEnumerable<Complex> polesEnumerable = indices.Select(i => Complex.FromPolar(1, Math.PI * i / (2 * order) + Math.PI / 2));
                poles = polesEnumerable.Select(x => 1 / (new Complex(Math.Sinh(mu) * x.Real, Math.Cosh(mu) * x.Imaginary))).ToArray();
            }

            //Calculate the gain
            Complex zeroMult = zeros.Aggregate(Complex.One, (x, y) => x * -y);
            Complex poleMult = poles.Aggregate(Complex.One, (x, y) => x * -y);
            double gain = (poleMult / zeroMult).Real;


            // Create zero-pole-gain form
            return new ZeroPoleGain(gain, zeros, poles);
        }


        /// <summary>
        /// Gets the analog prototype of an n-order Elliptic filter (lowpass with wc = 1Hz).  Similiar to ellipap function in Matlab.
        /// </summary>
        public static void Elliptic(int order, double passRipple, double stopRipple, out ZeroPoleGain zeroPoleGain)
        {
            if (order <= 0)
                throw new ArgumentException("Filter order must be greater than zero.");

            if (passRipple >= stopRipple)
                throw new ArgumentException("Stopband attenuation must be greater than passband ripple.");



            //Passband gain
            double gp = Math.Pow(10, -passRipple / 20);
            
            //Ripple factors
            double ep = Math.Sqrt(Math.Pow(10, passRipple / 10) - 1);
            double es = Math.Sqrt(Math.Pow(10, stopRipple / 10) - 1);

            throw new NotImplementedException();

            // Create zero-pole-gain form
            //zeroPoleGain = new ZeroPoleGain(gain, zeros, poles);
            //return;
        }



    }
}
