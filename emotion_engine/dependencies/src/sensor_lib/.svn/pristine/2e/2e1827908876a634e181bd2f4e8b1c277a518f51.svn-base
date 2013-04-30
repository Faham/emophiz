using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTLLiveCtrlLib;

using SensorLib.Filters;
using SensorLib.Sensors;

namespace SensorLib.ThoughtTechnologies
{

    //Takes unfiltered data and filters it
    public class BvpProcessor : IProcessor<float>
    {
        public BvpProcessor()
        {
            return;
        }


        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return inputSamplesPerSecond / 64;
        }


        //Takes unfiltered data and filters it
        public float[] ProcessData(float[] data)
        {
            throw new NotImplementedException();
            /*
            //Downsample
            //probably don't need to filter out high frequencies because we don't expect any to be present
            float[] newData = highPass.FilterData(downSampler.FilterData(data));

            //Check for heartbeat
            const float upperThreshold = 0.03f;
            const float lowerThreshold = 0.0f;
            foreach (float value in newData)
            {
                if (reachedLowerThreshold && value > upperThreshold)
                {
                    //!!Send event!
                    heartBeatEvent();
                    reachedLowerThreshold = false;
                }

                reachedLowerThreshold |= (value < lowerThreshold);
            }

            return newData;
             */
        }

        //!!public event HeartBeatHandler heartBeatEvent;

        readonly Filter downSampler = new Filters.RealTime.DownSampleFilter(64);
        //readonly Filter highPass = new Filters.RealTime.SingleZeroHPF();
        //readonly Filter lowPass = new Filters.RealTime.SingleZeroLPF();

        //!!bool reachedLowerThreshold = true;


    }

}
