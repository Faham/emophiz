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
using SensorLib.Mindset;

namespace SensorLibTesting
{
    class MindsetTestDevice : ITestDevice
    {

        public IDevice GetDevice()
        {
            // Get Mindset
            const int comPort = 3;
            Console.WriteLine("Connecting to Mindset (COM" + comPort + ")...");
            return MindsetDevice.Connect(comPort);
        }


        public ISensor GetSensor(IDevice device)
        {
            return ((MindsetDevice)device).CreateSensor("Test", true);
        }


        public String GetCurrentData(ISensor baseSensor, out int samples)
        {
            Sensor<BrainData> sensor = (Sensor<BrainData>)baseSensor;

            BrainData[] data = sensor.GetData(255);
            samples = data.Length;

            return String.Format("{0:0.000}", data.Last().signal);
        }


        public void RunSpecificTests()
        {
            return;
        }


    }
}
