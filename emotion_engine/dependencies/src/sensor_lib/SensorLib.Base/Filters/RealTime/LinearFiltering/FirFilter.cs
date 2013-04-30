using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Filters.FilterCreation;
using SensorLib.Util;


namespace SensorLib.Filters.RealTime
{
    /// <summary>
    /// A FIR filter is a filter which is made up of only zeros in the complex plane.
    /// </summary>
    public class FirFilter : LinearFilter
    {
        
        /// <summary>
        /// Creates an FIR filter from zeros-pole-gain form.  There must be no poles, and zeros must be made up of real and complex conjugate pairs.
        /// </summary>
        public FirFilter(ZeroPoleGain zeroPoleGain)
        {

            if (zeroPoleGain.Poles.Length > 0)
                throw new ArgumentException("An FIR filter cannot have any poles.");


            this.zeroPoleGain = zeroPoleGain;
            transferFunction = FilterModifiers.ConvertZeroPoleToTransferFunction(zeroPoleGain);


            this.order = zeroPoleGain.Zeros.Length;
            this.data = (Queue<double>)Enumerable.Repeat(0, order); //!!

            
            //Check whether this is linear phase
            double[] b = transferFunction.B;
            isLinearPhase = true;
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] != b[b.Length - 1 - i] && b[i] != -b[b.Length - 1 - i])
                {
                    isLinearPhase = false;
                    break;
                }
            }

            return;
        }
        
        
        /// <summary>
        /// Creates an FIR filter from the transfer function.
        /// </summary>
        public FirFilter(TransferFunction transferFunction)
        {
            if (transferFunction.A.Length > 0)
                throw new ArgumentException("An FIR filter cannot have any transfer function coefficients in the denominator.");

            this.transferFunction = transferFunction;
            this.order = transferFunction.B.Length - 1;

            this.data = (Queue<double>)Enumerable.Repeat(0, order); //!!

            //Check whether this is linear phase
            double[] b = transferFunction.B;
            isLinearPhase = true;
            for (int i = 0; i < b.Length; i++)
            {
                if (b[i] != b[b.Length - 1 - i] && b[i] != -b[b.Length - 1 - i])
                {
                    isLinearPhase = false;
                    break;
                }
            }

            return;
        }


        public override int Order { get { throw new NotImplementedException(); } }
        public override bool IsLinearPhase { get { return isLinearPhase; } }

        public override TransferFunction TransferFunction { get { return transferFunction; } }
        public override ZeroPoleGain ZeroPoleGain { get { throw new NotImplementedException(); } }
        

        public override Complex[] CalculateFreqResponse(int numFreqPoints)
        {
            // Get frequencies
            IEnumerable<Complex> freqs = Enumerable.Range(0, numFreqPoints).Select(x => Complex.FromPolar(1, 2 * Math.PI * x / numFreqPoints));

            return freqs.Select(x => Helpers.ComputePolynomial(transferFunction.B, x)).ToArray();
        }



        public override void Reset()
        {
            data.Clear();
            for (int i = 0; i < order; i++)
                this.data.Enqueue(0);

            return;
        }

        public override float[] FilterData(float[] input)
        {

            float[] output = new float[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                data.Enqueue(input[i]);
                output[i] = (float)data.Zip(transferFunction.B, (x, y) => x * y).Sum();
                data.Dequeue();
            }

            return output;
        }


        readonly TransferFunction transferFunction;
        readonly ZeroPoleGain zeroPoleGain;

        readonly int order;
        readonly bool isLinearPhase;

        //The data held in the registers
        readonly Queue<double> data;
        
    }
}
