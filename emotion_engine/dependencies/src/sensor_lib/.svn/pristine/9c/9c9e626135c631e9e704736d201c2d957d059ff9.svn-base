using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace SensorLib.Util
{
    //Creates a numerically controlled oscillator for testing purposes
    //NOTE: Does not have any relationship to time, only samples.
    class NCO
    {

        public NCO()
        {
            RadsPerSample = (float)Math.PI;

            phase = 0.0f;
        }

        public float[] ReadData(int maxSamples)
        {
            float[] data = new float[maxSamples];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (float)Math.Cos(phase);
                phase += RadsPerSample;
            }

            Thread.Sleep(100); //!!
            return data;
        }

        public float CyclesPerSample   //cycle/sample
        {
            get { return RadsPerSample / (2.0f * (float)Math.PI); }
            set { RadsPerSample = 2.0f * (float)Math.PI * value; }
        }

        public float RadsPerSample { get; set; }  //rads/sample

        public void Reset()
        {
            phase = 0.0f;
        }

        float phase;
        
    }
}
