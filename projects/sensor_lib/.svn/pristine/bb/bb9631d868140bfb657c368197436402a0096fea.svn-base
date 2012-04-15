using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Sensors;

namespace SensorLib.ThoughtTechnologies
{

    //Takes unfiltered data and filters it
    public class MuscleProcessor : IProcessor<float>
    {
        public MuscleProcessor()
        {
            normalizer = new Filters.RealTime.Normalizer();
            movingAverage = new SensorLib.Filters.RealTime.MovingAverage(100);

            return;
        }

        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return inputSamplesPerSecond;
        }

        //Takes unfiltered data and filters it
        public float[] ProcessData(float[] data)
        {
            //Downsample
            float[] newData = normalizer.FilterData(movingAverage.FilterData(data));  //probably don't need to filter out high frequencies because we don't expect any to be present
            return newData;
        }


        public readonly Filters.RealTime.Normalizer normalizer;
        public readonly Filters.RealTime.MovingAverage movingAverage;

    }

}
