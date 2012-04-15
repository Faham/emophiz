using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

using TTLLiveCtrlLib;

namespace SensorLib
{
    /*
    //!!Can make queueData be changed after constructor too
    abstract class TtlChannelSim : Sensor<float>, ITtlSensor
    {
        
        public TtlChannelSim(String name, Channel channel, bool queueData) : base(name, queueData)
        {
            this.channel = channel;

            randomGenerator = new Random();
            return;
        }


        public Channel Channel { get { return channel; } }

        public abstract float SampleRate { get; }

        /*
        //NOTE: Should not be called if encoder is not running.
        protected override float[] GetRawData(int maxSamples)
        {
            return new float[] { (float)randomGenerator.NextDouble() };
        }
        *//*
        
        readonly Channel channel;
        readonly Random randomGenerator;
        

    }
           */
}
