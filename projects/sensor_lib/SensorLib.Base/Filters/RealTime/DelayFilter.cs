using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{

    //Can be implemented as an all-pass ZeroPoleFilter with H(z) = z^(-delaySamples)
    public class DelayFilter : Filter
    {
        public DelayFilter() : this(0)
        { return; }


        public DelayFilter(int delay)
        {
            this.Delay = delay;

            dataQ = new Queue<float>();
            Reset();
            return;
        }


        /// <summary>
        /// Number of samples to delay the output.
        /// </summary>
        public int Delay { get; private set; }

        public override bool IsRealTime { get { return true; } }

        public override void Reset()
        {
            dataQ.Clear();
            for (int i = 0; i < Delay; i++)
            {
                dataQ.Enqueue(0.0f);
            }
            return;
        }
        
        public override float[] FilterData(float[] input)
        {
            if (input == null)
                throw new ArgumentNullException();

            float[] output = new float[input.Length];
            for(int i=0; i<input.Length; i++)
            {
                dataQ.Enqueue(input[i]);
                output[i] = dataQ.Dequeue();
            }

            return output;
        }


        Queue<float> dataQ;

    }
}
