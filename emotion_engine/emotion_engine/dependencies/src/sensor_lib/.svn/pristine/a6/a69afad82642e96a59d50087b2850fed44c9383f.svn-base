using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters
{
    /// <summary>
    /// Scales the bulk data to the between the min and max.
    /// </summary>
    public class BulkNormalizer : Filter
    {
        public BulkNormalizer() : this(1.0f, 0.0f)
        { return; }

        public BulkNormalizer(float max, float min)
        {
            Max = max;
            Min = min;
            return;
        }

        /// <summary>
        /// Max value the output data should contain.
        /// </summary>
        public float Max { get; private set; }

        /// <summary>
        /// Min value the output data should contain.
        /// </summary>
        public float Min { get; private set; }


        public override bool IsRealTime { get { return false; } }

        public override void Reset()
        {
            return;
        }


        public Sample<float>[] FilterData(Sample<float>[] data)
        {
            float[] values = FilterData(data.Select(x => x.Value).ToArray());
            return data.Zip(values, (x, y) => new Sample<float>(x.Time, y)).ToArray();
        }

        public override float[] FilterData(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException();

            //Get max value of data
            float dataMax = data.Max();
            float dataMin = data.Min();

            float scale = (Max - Min) / dataMax;

            float[] newData = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = (data[i] - dataMin) * scale + Min;
            }

            return newData;
        }


    }
}
