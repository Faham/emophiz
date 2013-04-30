using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using System.Threading;

using SensorLib.Base;
using SensorLib.Sensors;
using SensorLib.Inputs;

namespace SensorLib.Xna
{

    public class AudioDevice : Device<IAudioDeviceInfo, float>, IDevice
    {

        static AudioDevice()
        {
            //Start up a thread which updates the XNA framework
            Thread thread = new Thread(new ThreadStart(XnaFrameworkUpdate));
            thread.Start();
            return;
        }

        static void XnaFrameworkUpdate()
        {
            while (true)
            {
                FrameworkDispatcher.Update();
                Thread.Sleep(333);
            }
        }

        // Static Methods //

        public static IAudioDeviceInfo GetDefaultAudioDevice()
        {
            return new AudioDeviceInfo(Microphone.Default);
        }

        public static IAudioDeviceInfo[] GetAudioDevices()
        {
            List<IAudioDeviceInfo> audioDevices = new List<IAudioDeviceInfo>();
            foreach (Microphone microphone in Microphone.All)
            {
                audioDevices.Add(new AudioDeviceInfo(microphone));
            }

            return audioDevices.ToArray();
        }

        public static AudioDevice Connect(IAudioDeviceInfo info)
        {
            return new AudioDevice(Microphone.All.First(x => x.Name == info.Name));
        }



        // Constructors //

        protected AudioDevice(Microphone microphone)
        {
            this.microphone = microphone;
            this.microphone.BufferReady += new EventHandler<EventArgs>(BufferReadyHandler);

            info = new AudioDeviceInfo(microphone);


            const int bytesPerSample = 2;
            int bufferSize = (int)((microphone.BufferDuration.TotalSeconds * microphone.SampleRate * bytesPerSample) + 1);
            buffer = new byte[bufferSize];
            return;
        }


        // Public Properties //

        public override IAudioDeviceInfo Info { get { return info; } }



        // Public Methods //


        public IAudioSensor CreateSensor(String name, bool queueData)
        {
            AudioSensor sensor = new AudioSensor(microphone.SampleRate, name, queueData);
            base.CreateSensor(sensor);
            return sensor;
        }




        // Protected Methods //

        protected override void DeviceDispose() { return; }
        protected override void DeviceAddSensor(Sensors.Sensor<float> sensor) { return; }
        protected override void DeviceRemoveSensor(Sensors.Sensor<float> sensor) { return; }

        protected override void DeviceFirstSensorAdded()
        {
            //Start the microphone
            microphone.Start();
            return;
        }

        protected override void DeviceLastSensorRemoved()
        {
            //Stop the microphone
            microphone.Stop();
            return;
        }

        protected override void DeviceReceivedData(Sensors.Sensor<float> sensor, float[] data)
        {
            ((QueueInput<float>)sensor.Input).AddData(data);
            return;
        }


        

        // Private Methods //

        void BufferReadyHandler(object sender, EventArgs e)
        {
            GetData();
        }

        void GetData()
        {
            int bytes = microphone.GetData(buffer);
            if(bytes == 0)
                return;

            //Should have even number of bytes because each sample is 2 bytes
            Debug.Assert(bytes % 2 == 0);

            float[] data = new float[bytes / 2];
            for (int i = 0; i < data.Length; i++)
            {
                int value = (buffer[i+1] << 8) + buffer[i];  //little endian
                data[i] = value;
            }

            ReceivedData(data);
            return;
        }



        // Data //

        readonly IAudioDeviceInfo info;
        readonly Microphone microphone;
        readonly byte[] buffer;

    }
}
