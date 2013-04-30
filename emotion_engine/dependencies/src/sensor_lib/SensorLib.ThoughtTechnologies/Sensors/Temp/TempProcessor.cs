using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;

using SensorLib.Sensors;

namespace SensorLib.ThoughtTechnologies
{

    public class TempProcessor : IProcessor<float>
    {
        public TempProcessor()
        {
            downsampler = new Filters.RealTime.DownSampleFilter(64); //to get 32 from 2048
            normalizer = new Filters.RealTime.Normalizer();
            return;
        }


        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return inputSamplesPerSecond / 64;
        }


        //Takes unfiltered data and filters it
        public float[] ProcessData(float[] data)
        {
            //Downsample
            float[] newData = normalizer.FilterData(downsampler.FilterData(data));  //don't really need an anti-aliasing filter
            return newData;
        }

        public readonly Filters.RealTime.DownSampleFilter downsampler;
        public readonly Filters.RealTime.Normalizer normalizer;  //Good normalization values -- max: 0.25f, min: -0.05f


    }

}
