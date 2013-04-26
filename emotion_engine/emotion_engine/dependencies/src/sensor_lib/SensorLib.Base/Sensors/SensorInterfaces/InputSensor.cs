using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace SensorLib.Sensors
{
    /*
    //!!You can put any input into this sensor to test filters  (random noise, ramp, etc)
    class InputSensor : ISensor
    {
        public InputSensor(Inputs.IInput input_)
        {
            input = input_;
            return;
        }


        ~InputSensor()
        { return; }


        public void Start()
        { }

        public void Stop()
        { }

        public String SensorType { get { return "Input"; } }
        public float SampleRate { get { return 0.0f; } }

        public float[] GetData(int maxSamples)
        {
            return input.GetData(maxSamples);
        }

        readonly Inputs.IInput input;

    }
     */
}
