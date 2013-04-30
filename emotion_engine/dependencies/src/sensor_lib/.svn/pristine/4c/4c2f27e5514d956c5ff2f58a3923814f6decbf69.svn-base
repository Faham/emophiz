using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace SensorLib.Filters.RealTime
{
    public class SchmittTrigger : Filter
    {
        
        public SchmittTrigger(float upperThreshold_, float lowerThreshold_)
        {
            Debug.Assert(upperThreshold_ >= lowerThreshold);

            upperThreshold = upperThreshold_;
            lowerThreshold = lowerThreshold_;

            Reset();

            return;
        }


        public override bool IsRealTime { get { return true; } }

        public override void Reset()
        {
            outputValue = false;
            return;
        }

        public override float[] FilterData(float[] data)
        {
            if (data == null)
                return null;

            float[] newData = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                outputValue |= (data[i] > upperThreshold); //pull-up
                outputValue &= (data[i] >= lowerThreshold); //pull-down

                newData[i] = (outputValue) ? 1.0f : 0.0f; //convert bool to float
            }

            return newData;
        }

        readonly float upperThreshold;
        readonly float lowerThreshold;

        bool outputValue;
    }
}
