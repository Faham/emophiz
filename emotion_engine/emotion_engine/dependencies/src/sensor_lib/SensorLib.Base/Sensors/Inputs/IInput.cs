using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors.Inputs
{
    //Provides data
    //NOTE: GetData() must be blocking and must unblock when Disconnected() is called!
    public interface IInput<T> : IInput
    {

        T[] GetData(int maxSamples);

    }
}
