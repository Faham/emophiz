using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Util;

namespace SensorLib.Filters.NonUniform
{
    public class NonUniformMovingAverage : INonUniformFilter
    {

        /// <summary>
        /// A moving average whose window is based on time of non-uniformily spaced samples in time.
        /// </summary>
        /// <param name="sampleWidth_"></param>
        public NonUniformMovingAverage(TimeSpan windowWidth)
        {
            this.windowWidth = windowWidth;
            this.halfWindowWidth = TimeSpan.FromSeconds(windowWidth.TotalSeconds / 2f);
            return;
        }


        public TimeSpan WindowWidth { get { return windowWidth; } }


        public Sample<float>[] FilterData(Sample<float>[] input)
        {
            if (input == null)
                return null;

            Sample<float>[] output = new Sample<float>[input.Length];

            // Get values and duration of the values
            Sample<float>[] values = new Sample<float>[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                
                TimeSpan startTime;
                if (i == 0)
                    startTime = input[i].Time - halfWindowWidth;
                else
                    startTime = TimeSpan.FromSeconds((input[i - 1].Time + input[i].Time).TotalSeconds / 2f);

                values[i] = new Sample<float>(new Sample<float>(startTime, input[i].Value));
            }


            /*
            for (int i = 0; i < input.Length; i++)
            {
                
                float value = input[i].Value;
                TimeSpan startTime;
                TimeSpan endTime;

                if (i == 0)
                {
                    startTime = input[i].Time - windowWidth;
                    endTime = (input[i].Time + input[i + 1].Time).TotalSeconds / 2f;
                }
                else if (i == input.Length - 1)
                {
                    startTime = TimeSpan.FromSeconds((input[i - 1].Time + input[i].Time).TotalSeconds / 2f);
                    TimeSpan endTime = input[i].Time + windowWidth;
                    duration = (float)(endTime - startTime).TotalSeconds;
                }
                else
                {
                    startTime = TimeSpan.FromSeconds((input[i - 1].Time.TotalSeconds + input[i].Time.TotalSeconds) / 2f);
                    double endTime = (input[i].Time + input[i + 1].Time).TotalSeconds / 2f;
                    duration = (float)(endTime - startTime.TotalSeconds);
                }

                values[i] = new Pair<Sample<float>,TimeSpan>(new Sample<float>(startTime, value), duration);
            }
            */

            // Filter data
            int kernelStart = 0; // Points to first sample in kernel
            int kernelEnd = 0;  // Points to next sample that will be put into kernel
            for (int i = 0; i < input.Length; i++)
            {
                Sample<float> inputSample = input[i];
                TimeSpan windowStart = input[i].Time - halfWindowWidth;
                TimeSpan windowEnd = input[i].Time + halfWindowWidth;

                // Remove old samples from kernel
                while(kernelStart < input.Length - 1 && values[kernelStart+1].Time <= windowStart)
                {
                    kernelStart++;
                }

                // Add new samples to kernel
                while (kernelEnd < input.Length && values[kernelEnd].Time < windowEnd)
                {
                    kernelEnd++;
                }
                
                // Calculate weights for each sample
                float[] weights = new float[kernelEnd - kernelStart];
                for (int j = 0; j < weights.Length; j++)
                {
                    double startTime = values[kernelStart + j].Time.TotalSeconds;
                    double endTime;
                    if(kernelStart + j == values.Length - 1)
                        endTime = windowEnd.TotalSeconds;
                    else
                        endTime = values[kernelStart + 1 + j].Time.TotalSeconds;

                    double intersectStart = Math.Max(startTime, windowStart.TotalSeconds);
                    double intersectEnd = Math.Min(endTime, windowEnd.TotalSeconds);

                    weights[j] = (float)((intersectEnd - intersectStart) / windowWidth.TotalSeconds);
                }
                //weights[0] = (float)((values[kernelStart+1].Time - windowStart).TotalSeconds / windowWidth.TotalSeconds);
                //weights[weights.Length-1] = (float)((values[kernelEnd].Time - windowEnd).TotalSeconds / windowWidth.TotalSeconds);

                // Apply kernel
                float result = input.Skip(kernelStart).Take(kernelEnd - kernelStart).Zip(weights, (x,y) => x.Value * y).Sum();

                // Set output
                output[i] = new Sample<float>(input[i].Time, result);
            }

            return output;
        }


        readonly TimeSpan windowWidth;
        readonly TimeSpan halfWindowWidth;

    }
}
