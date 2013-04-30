using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib;

namespace SensorLib.Sensors
{
    
    /// <summary>
    /// Shifts the data of each sample according to the sensor's start time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SampleTimeShiftProcessor<T> : IProcessor<Sample<T>>
    {
        public SampleTimeShiftProcessor(DateTime deviceStartTime)
        {
            this.deviceStartTime = deviceStartTime;
            return;
        }

        public Sensor Sensor { get; set; }

        public Sample<T>[] ProcessData(Sample<T>[] data)
        {
            // Get amount of time to shift
            TimeSpan timeShift = Sensor.StartTime - deviceStartTime;

            // Shift data
            for (int i = 0; i < data.Length; i++)
                data[i].Time -= timeShift;

            return data.Where(x => x.Time > TimeSpan.Zero).ToArray();
        }


        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return inputSamplesPerSecond;
        }

        readonly DateTime deviceStartTime;
    }
    
}
