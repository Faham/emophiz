using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using SensorLib;
using SensorLib.Sensors;
using SensorLib.Tobii;


namespace SensorLibBasicExample
{
    static class TestEyeTracker
    {

        public static void Start()
        {
            Console.WriteLine("GUI Thread: " + Thread.CurrentThread.ManagedThreadId);

            EyeTracker.EyeTrackerAdded += new Action<IEyeTrackerInfo>(EyeTracker_EyeTrackerAdded);


            //Get default audio device
            Console.WriteLine("Finding eye trackers...");
            IEyeTrackerInfo[] devices = SensorLib.Tobii.EyeTracker.GetEyeTrackers();
            foreach (IEyeTrackerInfo eyeTrackerInfo in devices)
            {
                Console.WriteLine(eyeTrackerInfo.GivenName);
            }
            IEyeTrackerInfo device = devices.First();


            //Connect to eye tracker
            Console.WriteLine("Connecting to eye tracker...");
            IEyeTracker eyeTracker = EyeTracker.Connect(device);
            eyeTracker.Disconnected += new Action<IDevice>(eyeTracker_Disconnected);

            //Get sensor
            Console.WriteLine("Creating eye sensor...");
            SensorLib.Tobii.IEyeSensor sensor = eyeTracker.CreateSensor("Test", true);


            //Start sensor
            Console.WriteLine("Starting sensor");
            sensor.Start();


            //Gather Data
            Console.WriteLine("Gathering data");
            GatherData(sensor);


            //Stop sensor
            Console.WriteLine("Stopping sensor");
            sensor.Stop();

            //Remove Sensor
            Console.WriteLine("Removing sensor");
            sensor.Dispose();

            //Disconnect Device
            Console.WriteLine("Removing sensor");
            eyeTracker.Dispose();

            Console.WriteLine("Example completed successfully.");

            return;
        }

        static void eyeTracker_Disconnected(IDevice obj)
        {
            Console.WriteLine("Disconnected Thread: " + Thread.CurrentThread.ManagedThreadId);
        }


        static void EyeTracker_EyeTrackerAdded(IEyeTrackerInfo obj)
        {
            Console.WriteLine("Added Thread: " + Thread.CurrentThread.ManagedThreadId);
        }

        


        static void GatherData<T>(ISensor<T> sensor)
        {
            DateTime startTime = DateTime.Now;
            int samples = 0;

            //Get Data
            while (samples < 100)
            {
                T[] data = sensor.GetData(255);
                samples += data.Length;

                float samplesPerSecond = (float)(samples / (DateTime.Now - startTime).TotalSeconds);

                Console.WriteLine(samplesPerSecond + ": " + data[data.Length-1]);
            }

            return;
        }
        


    }
}
