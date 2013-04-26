using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib;

namespace SensorLib.Sensors
{
    
    /// <summary>
    /// Shifts the uniformly sampled data of each sample according to the sensor's start time by adding samples at the beginning of the output.
    /// NOTE: This is a hack to get around the fact that uniform samples do not have timestamps.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FixedSampleTimeShiftProcessor<T> : IProcessor<T>
    {
        public FixedSampleTimeShiftProcessor(DateTime deviceStartTime)
        {
            this.deviceStartTime = deviceStartTime;
            return;
        }

        public Sensor Sensor
        {
            get { return sensor; }

            set
            {
                
                if(sensor != null)
                {
                    sensor.Started -= sensor_Started;
                }

                if (!sensor.HasConstantSampleRate)
                    throw new Exception("Sensor must have a constant sample rate.  Use SampleTimeShiftProcessor for non-constant sample rates.");

                sensor = value;
                sensor.Started += new SensorEventHandler(sensor_Started);

                return;
            }
        }

        void sensor_Started(ISensor sensor)
        {
            // Get amount of time to shift
            TimeSpan timeShift = Sensor.StartTime - deviceStartTime;

            // Get number of samples needed
            int samples = (int)(sensor.SamplesPerSecond * timeShift.TotalSeconds);

            return;
        }


        public T[] ProcessData(T[] data)
        {
            T[] output;
            
            // Add samples
            if (samples > 0)
            {
                output = new T[samples + data.Length];
                for (int i = 0; i < samples; i++)
                    output[i] = default(T);
                
                data.CopyTo(output, samples);
                samples = 0;
            }
            else
            {
                output = data;
            }

            return output;
        }


        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return inputSamplesPerSecond;
        }

        
        readonly DateTime deviceStartTime;
        
        Sensor sensor;
        int samples;
    }
    
}
