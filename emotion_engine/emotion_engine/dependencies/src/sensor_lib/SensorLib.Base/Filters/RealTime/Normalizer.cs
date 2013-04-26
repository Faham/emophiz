using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Filters.RealTime
{
    public class Normalizer : Filter
    {
        public Normalizer() : this(1.0f, 0.0f)
        { return; }

        public Normalizer(float initMax_, float initMin_)
        {
            initMax = initMax_;
            initMin = initMin_;

            Max = initMax;
            Min = initMin;
            return;
        }

        public void SetMaxMin(float max_, float min_)
        {
            max = max_;
            min = min_;
            UpdateValues();
            return;
        }

        public float Max
        {
            get
            {
                return max;
            }

            set
            {
                max = value;
                UpdateValues();
                return;
            }
        }

        public float Min
        {
            get
            {
                return min;
            }

            set
            {
                min = value;
                UpdateValues();
                return;
            }
        }


        public override bool IsRealTime { get { return true; } }

        public override void Reset()
        {
            Max = initMax;
            Min = initMin;
            return;
        }

        public override float[] FilterData(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException();

            //!!Should we filter out high frequencies to prevent anti-aliasing when downsampling?
            float[] newData = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                newData[i] = (data[i] + bias) / scale;
            }

            return newData;
        }

        void UpdateValues()
        {
            //if (max <= min)
            //!!    throw new Exception("Invalid max/min specification.");

            bias = -min;
            scale = max - min;
            return;
        }


        float scale;
        float bias;

        float max;
        float min;

        readonly float initMax;
        readonly float initMin;
    }
}
