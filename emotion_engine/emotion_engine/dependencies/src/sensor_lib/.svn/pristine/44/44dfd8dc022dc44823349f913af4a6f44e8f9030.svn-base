using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Sensors;

namespace SensorLib.ThoughtTechnologies
{

    //Takes unfiltered data and filters it
    public class StrainProcessor : IProcessor<float>
    {
        public StrainProcessor()
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
            float[] newData = normalizer.FilterData(downsampler.FilterData(data));  //probably don't need to filter out high frequencies because we don't expect any to be present
            return newData;
        }


        public readonly Filters.RealTime.DownSampleFilter downsampler;
        public readonly Filters.RealTime.Normalizer normalizer;

    }

}
