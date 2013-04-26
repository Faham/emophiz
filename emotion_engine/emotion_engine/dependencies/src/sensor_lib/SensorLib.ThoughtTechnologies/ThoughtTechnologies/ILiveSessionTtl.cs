using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Sensors;

namespace SensorLib
{

    //Private interface for LiveSessionTtl classes (also nice if we decide to have a TTL-only library)
    interface ILiveSessionTtl : ITtlSensors
    {
        /*
        void StartChannels();
        void StopChannels();
        */


        void Dispose();  //!!It would be best if we didn't need this

        float GetBatteryPercentage(ITtlEncoder encoder);

        TtlEncoder[] GetEncoders();

        TtlSensor CreateSensor(ITtlEncoder encoder, SensorType type, Channel channel, String name, bool queueData);
        void RemoveSensor(ITtlSensor sensor);
    }
}
