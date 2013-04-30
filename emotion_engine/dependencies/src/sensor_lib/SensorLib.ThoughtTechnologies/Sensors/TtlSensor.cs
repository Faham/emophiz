using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Diagnostics;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

namespace SensorLib.ThoughtTechnologies
{
    //!!Can make queueData be changed after constructor too
    abstract class TtlSensor : Sensor<float>, ITtlSensor
    {
        
        public TtlSensor(IProcessor<float> processor, QueueInput<float> input, String name, Channel channel, bool queueData) : base(processor, input, name, queueData)
        {
            this.channel = channel;
            return;
        }


        // Shifts the time based on the time offset
        //!! NOTE: This is a hack to get around the fact that uniform samples do not have timestamps.
        public override void Start(DateTime startTime)
        {
            // Get amount of time to shift
            TimeSpan timeShift = DateTime.Now - startTime;

            // Get number of samples needed
            int samples = (int)(this.SamplesPerSecond * timeShift.TotalSeconds);

            // Add samples
            ((QueueInput<float>)Input).AddData(Enumerable.Repeat(0, samples).Select(x => (float)x).ToArray());

            base.Start(startTime);
            return;
        }

        public Channel Channel { get { return channel; } }
        public abstract SensorType SensorType { get; }

        readonly Channel channel;

    }
}
