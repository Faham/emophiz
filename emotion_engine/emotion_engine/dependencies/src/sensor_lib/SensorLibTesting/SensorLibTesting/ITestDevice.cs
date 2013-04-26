using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using SensorLib;
using SensorLib.Base;
using SensorLib.Sensors;
using SensorLib.ThoughtTechnologies;


namespace SensorLibTesting
{
    interface ITestDevice
    {

        IDevice GetDevice();

        ISensor GetSensor(IDevice device);

        String GetCurrentData(ISensor baseSensor, out int samples);

        void RunSpecificTests();
        
    }
}
