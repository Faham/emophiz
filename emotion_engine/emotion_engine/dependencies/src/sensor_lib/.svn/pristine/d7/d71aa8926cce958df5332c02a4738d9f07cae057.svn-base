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
    class TtlEncoderTestDevice : ITestDevice
    {

        public IDevice GetDevice()
        {
            // Get encoder
            Console.WriteLine("Finding encoders...");
            ITtlEncoderInfo[] deviceInfos = TtlEncoder.GetEncoders();
            foreach(ITtlEncoderInfo info in deviceInfos)
            {
                Console.WriteLine(info.ModelName);
            }
            ITtlEncoderInfo deviceInfo = deviceInfos.First();


            // Connect to TTL encoder
            Console.WriteLine("Connecting to encoder...");
            return TtlEncoder.Connect(deviceInfo);
        }


        public ISensor GetSensor(IDevice device)
        {
            return ((ITtlEncoder)device).CreateSensor("Test", SensorType.Raw, Channel.A, true);
        }


        public String GetCurrentData(ISensor baseSensor, out int samples)
        {
            Sensor<float> sensor = (Sensor<float>)baseSensor;

            float[] data = sensor.GetData(255);
            samples = data.Length;

            return String.Format("{0:0.000}", data.Last());
        }


        public void RunSpecificTests()
        {
            return;
        }


    }
}
