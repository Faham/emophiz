using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SensorLib;
using SensorLib.Sensors;
using SensorLib.Tobii;

namespace DeviceTests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            // Get device info
            IEyeTrackerInfo[] deviceInfos = EyeTracker.GetEyeTrackers();
            IEyeTrackerInfo deviceInfo = deviceInfos.First();

            // Connect to device
            device = EyeTracker.Connect(deviceInfo);

            // Create sensor
            sensor = device.CreateSensor("", false);
            sensor.Started += new SensorEventHandler(sensor_Started);
            sensor.Stopped += new SensorEventHandler(sensor_Stopped);
            sensor.DataAvailable += new DataAvailableHandler<Sample<EyeData>>(sensor_DataAvailable);

            return;
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            sensor.Stop();
            sensor.Dispose();
            device.Dispose();

            base.OnFormClosing(e);
            return;
        }

        
        
        void sensor_Started(ISensor sensor)
        {
            Console.WriteLine("Started");
            buttonStartStop.Text = "Stop";
            return;
        }

        void sensor_Stopped(ISensor sensor)
        {
            Console.WriteLine("Stopped");
            buttonStartStop.Text = "Start";
            return;
        }

        
        void sensor_DataAvailable(ISensor<Sample<EyeData>> sensor, Sample<EyeData>[] data)
        {
            Console.WriteLine(data.First().Time.TotalSeconds);
            return;
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (sensor.IsStarted)
                sensor.Stop();
            else
                sensor.Start();

            return;
        }


        readonly IEyeTracker device;
        readonly ISensor<Sample<EyeData>> sensor;
    }
}
