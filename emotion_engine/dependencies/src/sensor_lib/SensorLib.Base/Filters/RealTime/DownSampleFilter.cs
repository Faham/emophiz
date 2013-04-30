using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SensorLib.Filters.RealTime
{

    /// <summary>
    /// Downsamples the data without applying an anti-aliasing filter.  Use the DecimationFilter if you want to make sure the data does not alias.
    /// </summary>
    public class DownSampleFilter : Filter
    {
        public DownSampleFilter(int scale_)
        {
            Debug.Assert(scale_ != 0);

            scale = scale_;
            offset = 0;
            return;
        }

        public override bool IsRealTime { get { return true; } }

        public override void Reset()
        {
            offset = 0;
            return;
        }

        public override float[] FilterData(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException();

            //!!Should we filter out high frequencies to prevent anti-aliasing when downsampling?
            List<float> dataList = new List<float>();

            int sample;
            for (sample = scale - offset; sample < data.Length; sample += scale)
                dataList.Add(data[sample]);
            offset = data.Length - (sample - scale);

            return dataList.ToArray();
        }

        readonly int scale;

        int offset;
    }
}
