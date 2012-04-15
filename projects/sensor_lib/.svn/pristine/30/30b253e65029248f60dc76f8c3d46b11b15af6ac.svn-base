using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.ThoughtTechnologies
{
    public interface ITtlEncoder : SensorLib.Sensors.IDevice
    {


        float GetBatteryPercentage();
        ITtlSensor CreateSensor(String name, SensorType type, Channel channel, bool queueData);

        ITtlEncoderInfo Info { get; }

    }
}
