using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors
{
    //Just passes the data through with no changes
    public class NoneProcessor<T> : IProcessor<T>
    {
        public T[] ProcessData(T[] data)
        {
            return data;
        }


        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return inputSamplesPerSecond;
        }

    }
}
