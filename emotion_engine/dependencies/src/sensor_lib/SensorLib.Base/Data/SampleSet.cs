using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib
{
    //!! Can't we just use Sample<float[]> instead?
    /// <summary>
    /// Represents a collection of data points with a uniform sample rate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SampleSet<T>
    {

        public SampleSet(TimeSpan startTime, float samplesPerSecond, int length)
        {
            StartTime = startTime;
            SamplesPerSecond = samplesPerSecond;

            this.values = new T[length];

            return;
        }


        public SampleSet(TimeSpan startTime, float samplesPerSecond, T[] values)
        {
            StartTime = startTime;
            SamplesPerSecond = samplesPerSecond;

            Array.Copy(values, this.values, values.Length);

            return;
        }

        public TimeSpan StartTime { get; set; }
        public float SamplesPerSecond { get; set; }

        public T[] Values { get { return values; } }

        readonly T[] values;
    }
}
