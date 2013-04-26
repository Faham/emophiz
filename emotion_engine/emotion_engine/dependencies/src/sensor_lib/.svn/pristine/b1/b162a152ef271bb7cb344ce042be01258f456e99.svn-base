using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using SensorLib;
using SensorLib.Sensors;
/*
using SensorLib.Xna;


namespace SensorLibBasicExample
{

    using IAudioSensor = SensorLib.Xna.IAudioSensor;

    static class TestXnaAudio
    {
        
        
        public static void Start()
        {
            Console.WriteLine("GUI Thread: " + Thread.CurrentThread.ManagedThreadId);


            //Get default audio device
            Console.WriteLine("Finding default audio device...");
            AudioDevice device = AudioDevice.GetDefaultAudioDevice();


            //Connect to audio device
            Console.WriteLine("Connecting to audio device...");
            device.Disconnected += new Action<IDevice>(device_Disconnected);
            

            //Get sensor
            Console.WriteLine("Creating audio sensor...");
            IAudioSensor sensor = device.CreateSensor("Test", true);


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
            device.Dispose();
            
            Console.WriteLine("Example completed successfully.");
            
            return;
        }

        static void device_Disconnected(IDevice obj)
        {
            Console.WriteLine("Disconnected Thread: " + Thread.CurrentThread.ManagedThreadId);
            return;
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
*/