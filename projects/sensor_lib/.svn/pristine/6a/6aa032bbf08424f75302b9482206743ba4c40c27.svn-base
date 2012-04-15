using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

namespace SensorLib.ThoughtTechnologies
{
    class StrainSensor : TtlSensor
    {
        public StrainSensor(StrainProcessor processor, String name, Channel channel, float samplesPerSecond, bool queueData) : base(processor, new QueueInput<float>(samplesPerSecond), name, channel, queueData)
        {
            //Preconditions
            if (!IsValidSampleRate(samplesPerSecond))
                throw new Exception(String.Format("Invalid sample rate - {0}", samplesPerSecond));


            this.processor = processor;
            return;
        }


        public override SensorType SensorType { get { return SensorType.Strain; } }


        public void SetMaxMin(float max_, float min_)
        {
            processor.normalizer.SetMaxMin(max_, min_);
            return;
        }

        public float Max
        {
            get { return processor.normalizer.Max; }
            set { processor.normalizer.Max = value; }
        }

        public float Min
        {
            get { return processor.normalizer.Min; }
            set { processor.normalizer.Min = value; }
        }


        bool IsValidSampleRate(float sampleRate)
        {
            return sampleRate == 2048.0f;
        }



        readonly StrainProcessor processor;
        
    }
}
