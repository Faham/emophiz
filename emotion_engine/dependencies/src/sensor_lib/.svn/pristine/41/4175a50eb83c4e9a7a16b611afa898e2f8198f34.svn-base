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
    /// An IIR filter is a non-linear phase filter which is made up of zeros and poles in the complex plane.
    /// </summary>
    public class IirFilter : LinearFilter
    {

        /// <summary>
        /// Creates an IIR filter from zeros and poles.  The zeros and poles must be made up of real and complex conjugate pairs.
        /// </summary>
        public IirFilter(ZeroPoleGain zeroPoleGain)
        {
            Debug.Assert(zeroPoleGain.Zeros.Length == zeroPoleGain.Poles.Length);

            this.zeroPoleGain = zeroPoleGain;

            transferFunction = FilterModifiers.ConvertSosToTransferFunction(sosGain);
            sosGain = FilterModifiers.ConvertZeroPoleToSosFilter(zeroPoleGain);


            this.sections = new FilterChain(sosGain.Sections);
            this.order = sosGain.Sections.Sum(x => x.Order);


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
        /// Creates an IIR filter from second-order sections.
        /// </summary>
        public IirFilter(SosGain sosGain)
        {
            this.sosGain = sosGain;

            transferFunction = FilterModifiers.ConvertSosToTransferFunction(sosGain);
            zeroPoleGain = FilterModifiers.ConvertSosToZeroPole(sosGain);


            this.sections = new FilterChain(sosGain.Sections);
            //!!this.order = sosGain.Sections.Sum(x => x.Order);


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

        public SosGain SosGain { get { return sosGain; } }
        public override ZeroPoleGain ZeroPoleGain { get { return zeroPoleGain; } }
        public override TransferFunction TransferFunction { get { return transferFunction; } }
        


        public override Complex[] CalculateFreqResponse(int numFreqPoints)
        {
            // Get frequencies
            IEnumerable<Complex> freqs = Enumerable.Range(0, numFreqPoints).Select(x => Complex.FromPolar(1, Math.PI * x / numFreqPoints));


            return freqs.Select(x => Helpers.ComputePolynomial(transferFunction.B, x) / Helpers.ComputePolynomial(transferFunction.A, x)).ToArray();
        }



        public override void Reset()
        {
            sections.Reset();
            return;
        }

        public override float[] FilterData(float[] data)
        {
            // Apply gain to data
            data = Array.ConvertAll(data, x => (float)(sosGain.Gain * x));

            // Filter data with the second-order sections
            return sections.FilterData(data);
        }



        readonly FilterChain sections;
        readonly int order;

        readonly ZeroPoleGain zeroPoleGain;
        readonly TransferFunction transferFunction;
        readonly SosGain sosGain;

        readonly bool isLinearPhase;
    }
}
