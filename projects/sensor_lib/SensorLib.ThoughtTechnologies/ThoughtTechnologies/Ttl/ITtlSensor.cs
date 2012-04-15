using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SensorLib.Sensors
{
    public interface ITtlSensor : ISensor<float>
    {

        /// <summary>
        /// Sample rate in samples / sec
        /// </summary>
        float SampleRate { get; }

    }
}
