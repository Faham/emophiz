using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTLLiveCtrlLib;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

namespace SensorLib.ThoughtTechnologies
{
    //This class does no filtering on the signal
    class RawSensor : TtlSensor
    {
        public RawSensor(String name, Channel channel, float samplesPerSecond, bool queueData) : base(new NoneProcessor<float>(), new QueueInput<float>(samplesPerSecond), name, channel, queueData)
        { return; }

       
        public override SensorType SensorType { get { return SensorType.Raw; } }


    }
}
