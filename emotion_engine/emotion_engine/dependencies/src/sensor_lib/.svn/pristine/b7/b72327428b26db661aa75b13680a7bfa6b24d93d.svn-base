using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;
using SensorLib.Util;

namespace SensorLib.Filters.NonUniform
{
    
    public class NonUniformDifferentiator : INonUniformFilter
    {

        public NonUniformDifferentiator()
        {
            return;
        }

        /*
        public override void Reset()
        {
            oldData = new Sample<float>(TimeSpan.MinValue, 0);
            return;
        }
        */

        public Sample<float>[] FilterData(Sample<float>[] data)
        {
            Sample<float>[] output = new Sample<float>[data.Length];
            for (int i = 0; i < output.Length; i++)
            {
                Sample<float> firstValue;
                if (i == 0)
                    firstValue = oldData;
                else
                    firstValue = data[i - 1];

                Sample<float> secondValue = data[i];

                output[i] = new Sample<float>(secondValue.Time, (secondValue.Value - firstValue.Value) / (float)(secondValue.Time - firstValue.Time).TotalSeconds);
            }

            oldData = data.Last();
            return output;
        }

        Sample<float> oldData;
    }
}
