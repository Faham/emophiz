using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Filters;
using SensorLib.Filters.RealTime;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

namespace SensorLib.ThoughtTechnologies
{
    class HeartSensor : TtlSensor, IHeartBeatSensor
    {
        public HeartSensor(HeartProcessor processor, String name, Channel channel, float samplesPerSecond, bool queueData) : base(processor, new QueueInput<float>(samplesPerSecond), name, channel, queueData)
        {
            //Preconditions
            if (!IsValidSampleRate(samplesPerSecond))
                throw new Exception(String.Format("Invalid sample rate: {0}", samplesPerSecond));


            this.samplesPerSecond = samplesPerSecond;

            processor.heartBeatEvent += new HeartBeatHandler(this.OnHeartBeat);

            return;
        }



        public override SensorType SensorType { get { return SensorType.Heart; } }


        bool IsValidSampleRate(float samplesPerSecond)
        {
            return samplesPerSecond == 2048.0f;
        }


       


        //Calculate interbeatInterval
        void HeartBeat(DateTime time)
        {
            interbeatInterval = (time.Ticks - lastTime.Ticks) / 10000f;  //1 tick = 100ns;
            lastTime = time;

            //AddToOutputQueue(BeatsPerMinute);
            //Console.WriteLine("{0}", interbeatInterval);
            return;
        }

        DateTime lastTime = DateTime.Now;

        /// <summary>
        /// Time between heartbeats in milliseconds.
        /// </summary>
        public float InterbeatInterval { get { return interbeatInterval; } }
        public float BeatsPerMinute { get { return (1 / interbeatInterval) * 60000; } }

        //Event stuff
        public event HeartBeatHandler heartBeatEvent;
        void OnHeartBeat()
        {
            HeartBeat(DateTime.Now);
            if(heartBeatEvent != null)
                heartBeatEvent();
        }

        
        
        //List<float> interbeatIntervals;
        float interbeatInterval = 0.0f;
        readonly float samplesPerSecond;


    }
}
