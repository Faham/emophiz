using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;


using SensorLib.Sensors;
using SensorLib.ThoughtTechnologies;


namespace SensorLibBasicExample
{
    static class TestTtlEncoder
    {
        

        
        public static void Start()
        {
            Console.WriteLine("GUI Thread: " + Thread.CurrentThread.ManagedThreadId);


            //Get encoder
            Console.WriteLine("Finding encoders...");
            ITtlEncoderInfo[] devices = TtlEncoder.GetEncoders();
            foreach(ITtlEncoderInfo info in devices)
            {
                Console.WriteLine(info.ModelName);
            }
            ITtlEncoderInfo device = devices.First();


            //Connect to TTL encoder
            Console.WriteLine("Connecting to encoder...");
            ITtlEncoder encoder = TtlEncoder.Connect(device);
            encoder.Disconnected += new Action<IDevice>(encoder_Disconnected);
            
            //Get sensor
            Console.WriteLine("Creating raw sensor on channel A...");
            ITtlSensor sensor = encoder.CreateSensor("Test", SensorType.Raw, Channel.A, true);


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
            encoder.Dispose();
            
            Console.WriteLine("Example completed successfully.");
            
            return;
        }

        static void encoder_Disconnected(IDevice obj)
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
