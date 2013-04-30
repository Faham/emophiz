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


    /// <summary>
    /// Tests the device.
    /// TODO: Make tests assert that the events happen (and in proper order).
    /// </summary>
    class DeviceTests
    {


        static int mainThreadId;


        
        
        /// <summary>
        /// Tests normal user operation getting data from a sensor.
        /// </summary>
        /// <param name="testDevice"></param>
        public static void TestNormalOperation(ITestDevice testDevice)
        {

            Console.WriteLine("Starting normal operation test of " + testDevice.GetType().Name + "...");
            Console.WriteLine();


            // Get main thread ID
            mainThreadId = Thread.CurrentThread.ManagedThreadId;


            // Get Device
            Console.WriteLine("\nGetting device...");
            IDevice device = testDevice.GetDevice();
            device.Disconnected += new Action<IDevice>(device_Disconnected);


            // Create sensor
            Console.WriteLine("\nCreating raw sensor on channel A...");
            ISensor sensor = testDevice.GetSensor(device);
            sensor.Started += new SensorEventHandler(sensor_Started);
            sensor.Stopped += new SensorEventHandler(sensor_Stopped);
            sensor.Removed += new SensorEventHandler(sensor_Removed);
            sensor.Dropped += new SensorEventHandler(sensor_Dropped);


            // Start sensor
            Console.WriteLine("\nStarting sensor...");
            sensor.Start();


            // Gather Data
            Console.WriteLine("\nGathering data...");
            GatherData(testDevice, sensor);


            // Stop sensor
            Console.WriteLine("\nStopping sensor...");
            sensor.Stop();

            // Remove Sensor
            Console.WriteLine("\nRemoving sensor...");
            sensor.Dispose();

            // Disconnect Device
            Console.WriteLine("\nRemoving device...");
            device.Dispose();


            // Remove device events
            device.Disconnected -= device_Disconnected;
            device.Dropped -= device_Dropped;


            // Remove sensor events
            sensor.Started -= sensor_Started;
            sensor.Stopped -= sensor_Stopped;
            sensor.Removed -= sensor_Removed;
            sensor.Dropped -= sensor_Dropped;
        


            return;
        }



        /// <summary>
        /// Tests disconnecting of the device with sensors still connected.
        /// </summary>
        /// <param name="testDevice"></param>
        public static void TestDeviceDisconnection(ITestDevice testDevice)
        {
            Console.WriteLine("Starting device disconnection test of " + testDevice.GetType().Name + "...");
            Console.WriteLine();

            // Get main thread ID
            mainThreadId = Thread.CurrentThread.ManagedThreadId;


            // Get Device
            Console.WriteLine("\nGetting device...");
            IDevice device = testDevice.GetDevice();
            device.Disconnected += new Action<IDevice>(device_Disconnected);


            // Create sensor
            Console.WriteLine("\nCreating raw sensor on channel A...");
            ISensor sensor = testDevice.GetSensor(device);
            sensor.Started += new SensorEventHandler(sensor_Started);
            sensor.Stopped += new SensorEventHandler(sensor_Stopped);
            sensor.Removed += new SensorEventHandler(sensor_Removed);
            sensor.Dropped += new SensorEventHandler(sensor_Dropped);


            // Start sensor
            Console.WriteLine("\nStarting sensor...");
            sensor.Start();


            // Disconnect Device
            Console.WriteLine("\nRemoving device...");
            device.Dispose();


            return;
        }


        /// <summary>
        /// Tests disconnecting of the device with sensors still connected.
        /// </summary>
        /// <param name="testDevice"></param>
        public static void TestDeviceDrop(ITestDevice testDevice)
        {
            Console.WriteLine("Starting device drop test of " + testDevice.GetType().Name + "...");
            Console.WriteLine();

            // Get main thread ID
            mainThreadId = Thread.CurrentThread.ManagedThreadId;


            // Get Device
            Console.WriteLine("\nGetting device...");
            IDevice device = testDevice.GetDevice();
            device.Disconnected += new Action<IDevice>(device_Disconnected);
            device.Dropped += new Action<IDevice>(device_Dropped);

            // Create sensor
            Console.WriteLine("\nCreating raw sensor on channel A...");
            ISensor sensor = testDevice.GetSensor(device);
            sensor.Started += new SensorEventHandler(sensor_Started);
            sensor.Stopped += new SensorEventHandler(sensor_Stopped);
            sensor.Removed += new SensorEventHandler(sensor_Removed);
            sensor.Dropped += new SensorEventHandler(sensor_Dropped);


            // Start sensor
            Console.WriteLine("\nStarting sensor...");
            sensor.Start();

            return;
        }

        
        
        /// <summary>
        /// Tests the device using the device-specific tests.
        /// </summary>
        /// <param name="testDevice"></param>
        public static void TestDeviceSpecificTests(ITestDevice testDevice)
        {
            // Do device specific tests
            Console.WriteLine("Starting device-specific tests...");
            testDevice.RunSpecificTests();

            return;
        }


        // Device Events


        static void device_Disconnected(IDevice obj)
        {
            Console.WriteLine("** Device Disconnected Event **");
            
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if(threadId != mainThreadId)
                throw new Exception("Error: Thread ID is not the same as the main thread ID.");
            
            return;
        }


        static void device_Dropped(IDevice obj)
        {
            Console.WriteLine("** Device Dropped Event **");

            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadId != mainThreadId)
                throw new Exception("Error: Thread ID is not the same as the main thread ID.");

            return;
        }


        // Sensor Events



        static void sensor_Started(ISensor sensor)
        {
            Console.WriteLine("** Sensor Started Event **");
            
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadId != mainThreadId)
                throw new Exception("Error: Thread ID is not the same as the main thread ID.");

            return;
        }

        static void sensor_Stopped(ISensor sensor)
        {
            Console.WriteLine("** Sensor Stopped Event **");

            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadId != mainThreadId)
                throw new Exception("Error: Thread ID is not the same as the main thread ID.");

            return;
        }


        static void sensor_Removed(ISensor sensor)
        {
            Console.WriteLine("** Sensor Removed Event **");

            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadId != mainThreadId)
                throw new Exception("Error: Thread ID is not the same as the main thread ID.");

            return;
        }


        static void sensor_Dropped(ISensor sensor)
        {
            Console.WriteLine("** Sensor Dropped Event **");

            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (threadId != mainThreadId)
                throw new Exception("Error: Thread ID is not the same as the main thread ID.");

            return;
        }

        


        static void GatherData(ITestDevice testDevice, ISensor sensor)
        {
            const int dataPrintSeconds = 5;

            DateTime startTime = DateTime.Now;

            int totalSamples = 0;

            //Get Data for a certain amount of time
            while ((DateTime.Now - startTime).TotalSeconds < dataPrintSeconds)
            {
                int samples;
                String currentData = testDevice.GetCurrentData(sensor, out samples);

                totalSamples += samples;

                float samplesPerSecond = (float)(totalSamples / (DateTime.Now - startTime).TotalSeconds);
                Console.WriteLine(String.Format("{0:0.000}: {1}", samplesPerSecond, currentData));
            }

            return;
        }
        


    }
}
