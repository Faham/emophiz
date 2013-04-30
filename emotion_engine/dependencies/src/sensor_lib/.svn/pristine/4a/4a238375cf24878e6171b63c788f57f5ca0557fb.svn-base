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
using SensorLib.Xna;


namespace SensorLibTesting
{
    class XnaAudioTestDevice : ITestDevice
    {

        public IDevice GetDevice()
        {
            // Get microphone
            Console.WriteLine("Finding microphones...");
            IAudioDeviceInfo[] deviceInfos = AudioDevice.GetAudioDevices();
            foreach(IAudioDeviceInfo info in deviceInfos)
            {
                Console.WriteLine(info.Name);
            }


            // Connect to TTL encoder
            Console.WriteLine("Connecting to default microphone...");
            return AudioDevice.Connect(AudioDevice.GetDefaultAudioDevice());
        }


        public ISensor GetSensor(IDevice device)
        {
            return ((AudioDevice)device).CreateSensor("Test", true);
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
