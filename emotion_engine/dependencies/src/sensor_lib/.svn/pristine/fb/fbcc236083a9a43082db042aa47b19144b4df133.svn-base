using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Sensors;
using SensorLib.Util;


namespace SensorLib.Filters
{
    
    /// <summary>
    /// Converts a stream of samples to regular intervals.
    /// </summary>
    public class InterpolatingFilter
    {


        public static float GetMinimumSamplesPerSecond(Sample<float>[] data)
        {
            // Find minimum time between samples
            TimeSpan minTime = data.Skip(1).Zip(data, (x, y) => x.Time - y.Time).Min();
            return (float)(1.0f / minTime.TotalSeconds);
        }

        
        /// <summary>
        /// Creates a new interpolating filter with sample rate depending on data given.
        /// </summary>
        public InterpolatingFilter()
        {
            AutoSampleRate = true;
            return;
        }

        /// <summary>
        /// Creates a new interpolating filter with manually set sample rate.
        /// </summary>
        /// <param name="samplesPerSecond"></param>
        public InterpolatingFilter(float samplesPerSecond)
        {
            SamplesPerSecond = samplesPerSecond;
            return;
        }

        /// <summary>
        /// Specifies whether to use the minimum sample rate achieved from data.
        /// </summary>
        public bool AutoSampleRate { get; set; }

        public float SamplesPerSecond
        {
            get { return samplesPerSecond; }
            set
            {
                samplesPerSecond = value;
                AutoSampleRate = false;
            }
        }


        //!! Need to eliminate some high frequencies?
        //!! Need to make real-time!
        //!! Does not take into account the offset!
        //!! Should allow cubic spline (matlab: spline() )
        //!! Should start samples at the first uniform sample?
        public SampleSet<float> FilterData(Sample<float>[] input)
        {

            if (AutoSampleRate)
            {
                // Find minimum time between samples
                samplesPerSecond = GetMinimumSamplesPerSecond(input);
            }

            TimeSpan minTime = TimeSpan.FromSeconds(1.0f / samplesPerSecond);


            // Get new sample times
            float[] newSampleTimesArray = 
                Enumerable.Range(0, int.MaxValue).
                Select(x => (float)(x / samplesPerSecond)).
                TakeWhile(x => x <= input.Last().Time.TotalSeconds).
                ToArray();
            //float[] newSampleTimesArray = 
            //    Enumerable.Range(0, int.MaxValue).
            //    Select(x => (float)(input.First().Time.TotalSeconds + (x / samplesPerSecond))).
            //    TakeWhile(x => x <= input.Last().Time.TotalSeconds).
            //    ToArray();


            // Create a list of new samples
            SampleSet<float> output = new SampleSet<float>(TimeSpan.Zero, samplesPerSecond, newSampleTimesArray.Length);
            int outputCount = 0;
            
            // Add first point
            output.Values[0] = input.First().Value;
            outputCount++;

            // Linearly interpolate points
            Sample<float> lastSample = new Sample<float>(TimeSpan.Zero, 0);
            foreach (Sample<float> sample in input)
            {
                double m = (sample.Value - lastSample.Value) / (sample.Time - lastSample.Time).TotalSeconds;
                double b = lastSample.Value;
                double p = lastSample.Time.TotalSeconds;

                while (outputCount < newSampleTimesArray.Length && newSampleTimesArray[outputCount] <= sample.Time.TotalSeconds)
                {
                    float newTime = newSampleTimesArray[outputCount];

                    // Interpolate value
                    output.Values[outputCount] = (float)((newTime - p) * m + b);
                    outputCount++;
                    
                }

                lastSample = sample;
            }


            //Return filtered data
            return output;
        }

        

        float samplesPerSecond;

    }
}
