using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;

using SensorLib.Sensors;
using SensorLib.Inputs;

namespace SensorLib.Xna
{
    class AudioSensor : Sensor<float>, IAudioSensor
    {

        public AudioSensor(float sampleRate, String name, bool queueData) : base(new NoneProcessor<float>(), new QueueInput<float>(sampleRate), name, queueData)
        {
            return;
        }

    }
}
