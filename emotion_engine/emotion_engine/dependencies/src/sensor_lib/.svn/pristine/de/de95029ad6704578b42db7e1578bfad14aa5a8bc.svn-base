using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTLLiveCtrlLib;

using SensorLib.Filters;
using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

namespace SensorLib.ThoughtTechnologies
{
    class BvpSensor : TtlSensor
    {
        public BvpSensor(BvpProcessor processor, String name, Channel channel, float samplesPerSecond, bool queueData) : base(processor, new QueueInput<float>(samplesPerSecond), name, channel, queueData)
        {
            //Preconditions
            if (!IsValidSampleRate(samplesPerSecond))
                throw new Exception(String.Format("Invalid sample rate - {0}", samplesPerSecond));


            //!!processor.heartBeatEvent += new HeartBeatHandler(this.OnHeartBeat);
            
            return;
        }


        public override SensorType SensorType { get { return SensorType.Bvp; } }

        
        bool IsValidSampleRate(float sampleRate)
        {
            return sampleRate == 2048.0f;
        }



        
        //Event stuff
        public event HeartBeatHandler heartBeatEvent;

        void OnHeartBeat()
        {
            heartBeatEvent();
        }


    }
}
