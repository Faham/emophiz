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
using SensorLib.Tobii;


namespace SensorLibTesting
{

    /// <summary>
    /// TODO: Eye calibration test.
    /// </summary>
    class EyeTrackerTestDevice : ITestDevice
    {

        public IDevice GetDevice()
        {
            // Get eye tracker
            Console.WriteLine("Finding eye tracker...");
            IEyeTrackerInfo[] deviceInfos = EyeTracker.GetEyeTrackers();
            foreach (IEyeTrackerInfo info in deviceInfos)
            {
                Console.WriteLine(info.Model);
            }
            IEyeTrackerInfo deviceInfo = deviceInfos.First();

            
            // Connect to TTL encoder
            Console.WriteLine("Connecting to eye tracker...");
            IEyeTracker device = EyeTracker.Connect(deviceInfo);

            // Calibrate eye tracker
            Console.WriteLine("Calibrating eye tracker...");
            device.Calibrate();

            return device;
        }


        public ISensor GetSensor(IDevice device)
        {
            return ((IEyeTracker)device).CreateSensor("Test", true);
        }


        public String GetCurrentData(ISensor baseSensor, out int samples)
        {
            Sensor<EyeData> sensor = (Sensor<EyeData>)baseSensor;

            EyeData[] data = sensor.GetData(255);
            samples = data.Length;

            return String.Format("{0:0.000}", data.Last().left.eyePos.X);
        }


        public void RunSpecificTests()
        {
            return;
        }


    }
}
