using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors.Inputs
{
    /*
    class RandomInput : IInput<float>
    {
        public RandomInput()
        {
            randomGenerator = new Random(); //!!seed!
            return;
        }

        public void Reset() { }

        public float[] GetData(int maxSamples)
        {
            float[] data = new float[maxSamples];
            for(int i=0; i<data.Length; i++)
                data[i] = (float)randomGenerator.NextDouble();
            return data;
        }

        readonly Random randomGenerator;
    }
     */
}
