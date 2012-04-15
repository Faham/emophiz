using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib
{
    [Serializable]
    public class Sample<T>
    {
        public Sample()
        {
            Time = TimeSpan.Zero;
            Value = default(T);
            return;
        }

        public Sample(Sample<T> sample)
        {
            Time = sample.Time;
            Value = sample.Value;
            return;
        }

        public Sample(TimeSpan time, T value)
        {
            Time = time;
            Value = value;
            return;
        }

        public TimeSpan Time { get; set; }
        public T Value { get; set; }
    }
}
