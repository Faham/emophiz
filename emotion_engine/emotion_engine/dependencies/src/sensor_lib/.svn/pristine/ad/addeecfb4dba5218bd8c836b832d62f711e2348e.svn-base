using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

using TTLLiveCtrlLib;

namespace SensorLib
{
    /*
    //!!Can make queueData be changed after constructor too
    abstract class TtlChannel : Sensor<float>, ITtlChannelFront, ITtlChannelBack, ITtlSensor
    {
        
        public TtlChannel(String name, Channel channel, TTLChannel ttlChannel, bool queueData) : base(name, queueData)
        {
            this.channel = channel;
            this.ttlChannel = ttlChannel;

            return;
        }

        public override ManufacturerType Manufacturer { get { return ManufacturerType.Ttl; } }
        public Channel Channel { get { return channel; } }
        internal TTLChannel TTLChannel { get { return ttlChannel; } }

        public abstract float SampleRate { get; }


        //NOTE: Should not be called if encoder is not running.
        protected override float[] GetRawData(int maxSamples)
        {
            
            //Check if data is available
            int available = ttlChannel.SamplesAvailable;
            if (available > maxSamples) available = maxSamples;

            if (available <= 0)
                return new float[0];

            //Throw away the data if the sensor is not started
            if (!IsStarted)
                return new float[0];

            //Read data
            //float[] data = new float[available];
            //liveChannel.ReadData(out data[0], ref available);
            float[] rawData = (float[])ttlChannel.ReadDataVT(available);

            return rawData;
        }
        
        readonly TTLChannel ttlChannel;
        readonly Channel channel;
        

    }
     * */
}
