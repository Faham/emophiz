using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors.Inputs
{

    public interface IInput
    {

        event Action<IInput> Disconnected;

        void Disconnect();

        float SamplesPerSecond { get; }

        void Start();
        void Stop();
    }
}
