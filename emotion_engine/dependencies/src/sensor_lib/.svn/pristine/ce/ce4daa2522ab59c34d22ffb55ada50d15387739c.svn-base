using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using SensorLib.Util;

namespace SensorLib.Filters
{

    /// <summary>
    /// A filter takes input data, transforms it, and outputs the transformed data.  It only iterates over the input data once.
    /// </summary>
    public abstract class Filter
    {


        static public float[] Square(float[] data)
        {
            if (data == null)
                return null;

            float[] newData = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = data[i] * data[i];
            }
            return newData;
        }

        public float[] FilterData(float value)
        {
            float[] data = { value };
            return FilterData(data);
        }

        /// <summary>
        /// Returns whether the filter is a real-time filter.  See Nomenclature.cs for info on "real-time" filters.
        /// </summary>
        public abstract bool IsRealTime { get; }

        public abstract void Reset();


        //!!public abstract IEnumerable<T> FilterData(IEnumerable<T> data);
        public abstract float[] FilterData(float[] data);
        //!!public abstract Sample<float>[] FilterData(Sample<float>[] data);

    }
}
