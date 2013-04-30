using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;

namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// Second-order section (Direct Form II).  Second-order sections are also known as digital biquad filters.
    /// </summary>
    public class Sos : LinearFilter
    {
        
        public Sos(string matlabCoefs)
        {
            data = new double[2];
            double[] b = new double[3];
            double[] a = new double[3];
            

            char[] delimiters = { ' ' };
            string[] nums = matlabCoefs.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(nums.Length == 6);
            
            b[0] = float.Parse(nums[0]);
            b[1] = float.Parse(nums[1]);
            b[2] = float.Parse(nums[2]);

            a[0] = float.Parse(nums[3]);
            a[1] = float.Parse(nums[4]);
            a[2] = float.Parse(nums[5]);

            this.transferFunction = new TransferFunction(b, a);

            // Get zero-pole-gain form
            throw new NotImplementedException();
            return;
        }
        

        /// <summary>
        /// Converts two zeros and two poles into a second-order section.  Assumes the final coefficients will be real.
        /// </summary>
        /// <param name="zeros"></param>
        /// <param name="poles"></param>
        public Sos(ZeroPoleGain zeroPoleGain)
        {
            Complex[] zeros = zeroPoleGain.Zeros;
            Complex[] poles = zeroPoleGain.Poles;

            //!!Does this really need to be true?
            if (zeros.Length != poles.Length)
                throw new Exception("There must be equal numbers of poles and zeros.");

            int order = zeros.Length;

            if (order != 1 && order != 2)
                throw new Exception("There must be either one or two zeros and poles.");


            //!! Make sure the zeros and poles are real or conjugate pairs


            this.data = new double[order];

            this.zeroPoleGain = zeroPoleGain;
            this.transferFunction = ConvertZeroPoleToTransferFunction(this.zeroPoleGain);

            return;
        }


        public override int Order { get { throw new NotImplementedException(); } }
        public override bool IsLinearPhase { get { throw new NotImplementedException(); } }


        public override TransferFunction TransferFunction { get { return transferFunction; } }
        public override ZeroPoleGain ZeroPoleGain { get { return zeroPoleGain; } }



        
        public override Complex[] CalculateFreqResponse(int numFreqPoints)
        {
            // Get frequencies
            IEnumerable<Complex> freqs = Enumerable.Range(0, numFreqPoints).Select(x => Complex.FromPolar(1, Math.PI * x / numFreqPoints));

            return freqs.Select(x => Helpers.ComputePolynomial(transferFunction.B, x) / Helpers.ComputePolynomial(transferFunction.A, x)).ToArray();
        }

        

        public override void Reset()
        {
            data.Initialize();
            return;
        }

        public override float[] FilterData(float[] input)
        {
            Debug.Assert(input != null);

            //Create output array
            float[] output = new float[input.Length];

            double[] b = transferFunction.B;
            double[] a = transferFunction.A;

            //Loop through all input values
            for(int i=0; i<output.Length; i++)
            {
                double firstSum = data[0] * -a[1] + data[1] * -a[2] + input[i];

                output[i] = (float)((firstSum*b[0] + data[0]*b[1] + data[1]*b[2]) / a[0]);

                data[1] = data[0];
                data[0] = firstSum;
            }

            return output;
        }



        /// <summary>
        /// Converts the zeros and poles into transfer coefficients.
        /// </summary>
        /// <param name="zeros"></param>
        /// <param name="poles"></param>
        static TransferFunction ConvertZeroPoleToTransferFunction(ZeroPoleGain zeroPoleGain)
        {

            // Number of poles and zeros must be the same
            Debug.Assert(zeroPoleGain.Zeros.Length == zeroPoleGain.Zeros.Length);
            int order = zeroPoleGain.Zeros.Length;

            // Can only have an order of 1 or 2
            Debug.Assert(order == 1 || order == 2);

            
            // Create our arrays of coefficients
            double[] b = new double[order + 1];
            double[] a = new double[order + 1];
            
            // Calculate the coefficients
            switch(order)
            {
                case 1:
                    {
                        Complex zero = zeroPoleGain.Zeros[0];
                        Complex pole = zeroPoleGain.Poles[0];
                        
                        // Make sure zero and pole are both real
                        if (!zero.IsRealWithTolerance || !pole.IsRealWithTolerance)
                            throw new Exception("Zero and pole must be real in a first-order filter.");

                        // Convert pole and zero into transfer function coefficients
                        // (x - r1) -> x - r1
                        b[0] = 1;
                        b[1] = -zero.Real;
                        a[0] = 1;
                        a[1] = -pole.Real;

                        break;
                    }

                case 2:
                    {
                        Complex[] zeros = zeroPoleGain.Zeros;
                        Complex[] poles = zeroPoleGain.Poles;

                        // Make sure we have complex conjugate pairs or real
                        bool zerosBothReal = zeros[0].IsRealWithTolerance && zeros[1].IsRealWithTolerance;
                        bool zerosConjugates = (zeros[0] + zeros[1]).IsRealWithTolerance;
                        bool polesBothReal = poles[0].IsRealWithTolerance && poles[1].IsRealWithTolerance;
                        bool polesConjugates = (poles[0] + poles[1]).IsRealWithTolerance;

                        if (!zerosBothReal && !zerosConjugates)
                            throw new Exception("Could not pair the zeros.");

                        if (!polesBothReal && !polesConjugates)
                            throw new Exception("Could not pair the poles.");


                        // Convert poles and zeros into transfer function coefficients
                        Helpers.RootsToPoly(zeros[0], zeros[1], out b[0], out b[1], out b[2]);
                        Helpers.RootsToPoly(poles[0], poles[1], out a[0], out a[1], out a[2]);

                        break;
                    }

                default: Debug.Assert(false); throw new Exception();
            }

            // Incorporate gain into the numerator of the transfer function
            b = Array.ConvertAll(b, x => zeroPoleGain.Gain * x);

            return new TransferFunction(b, a);
        }


        static ZeroPoleGain ConvertTransferFunctionToZeroPole(TransferFunction transferFunction)
        {
            throw new NotImplementedException();

            Complex[] zeros = new Complex[2];
            Helpers.PolyToRoots(transferFunction.B[0], transferFunction.B[1], transferFunction.B[2], out zeros[0], out zeros[1]);

            Complex[] poles = new Complex[2];
            Helpers.PolyToRoots(transferFunction.A[0], transferFunction.A[1], transferFunction.A[2], out poles[0], out poles[1]);

        }


        readonly TransferFunction transferFunction;
        readonly ZeroPoleGain zeroPoleGain;

        readonly double[] data;

    }
}
