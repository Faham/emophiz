using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;
using SensorLib.Util;

namespace SensorLib.Filters.RealTime
{
    //A differentiator is a single zero highpass filter
    public class Differentiator : Filter, IUniformFilter
    {

        public Differentiator()
        {
            return;
        }

        public override bool IsRealTime { get { return true; } }

        public override void Reset()
        {
            oldData = 0;
            return;
        }

        public override float[] FilterData(float[] data)
        {
            float[] output = new float[data.Length];
            for (int i = 0; i < output.Length; i++)
            {
                if (i == 0)
                    output[i] = data[0] - oldData;
                else
                    output[i] = data[i] - data[i - 1];
            }

            oldData = data.Last();
            return output;
        }

        float oldData;
    }
}
