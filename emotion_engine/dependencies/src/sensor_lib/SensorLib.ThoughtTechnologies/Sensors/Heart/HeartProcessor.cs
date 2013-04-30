using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;
using SensorLib.Filters.RealTime;

using SensorLib.Sensors;

namespace SensorLib.ThoughtTechnologies
{

    //Takes unfiltered data and filters it
    public class HeartProcessor : IProcessor<float>
    {
        public HeartProcessor()
        { return; }


        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return 0.0f;
        }


        //Takes unfiltered data and filters it
        public float[] ProcessData(float[] data)
        {
            //Highpass -> Envelope Detector
            float[] filteredData = lowPass.FilterData(Filter.Square(highPass.FilterData(powerNotch.FilterData(data))));

            //Normalizer -> Schmitt Trigger
            float[] pulseData = normalizer.FilterData(filteredData);

            float[] triggerData = trigger.FilterData(pulseData);


            //Check for heartbeat
            float[] peak = differentiator.FilterData(triggerData);

            /*
            float max = 0.0f;
            for (int i = 0; i < triggerData.Length; i++)
            {
                if (peak[i] == -1.0f)
                {
                    //normalizer.Max = normLowPass.FilterData(max)[0];
                    
                    
                    //Console.WriteLine("max: {0}", max);
                    max = 0.0f;
                }
                else
                    max = Math.Max(max, filteredData[i]);
            }
            */

            foreach (float value in peak)
            {
                if (value == 1.0f)
                {
                    heartBeatEvent();
                }
            }

            return filteredData;
        }


        public event HeartBeatHandler heartBeatEvent;

        //Signal filters
        Filter differentiator = new Differentiator();
        Filter highPass = new HeartOffsetHPF();
        Filter lowPass = new MovingAverage(100);

        Filter trigger = new SchmittTrigger(0.15f, 0.075f); //(0.00015f, 0.00005f);

        Filter powerNotch = new PowerNotchFilter();

        Filters.RealTime.Normalizer normalizer = new Normalizer(0.001f, 0.0f);


        //Adaptive Normalization Filters
        Filter normLowPass = new MovingAverage(3);

        //Filter flatFilter = new FlatFilter();

    }

}
