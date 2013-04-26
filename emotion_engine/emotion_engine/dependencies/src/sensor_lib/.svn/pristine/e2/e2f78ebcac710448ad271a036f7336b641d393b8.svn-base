using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace SensorLib.Filters.RealTime
{
    public class MovingAverage : Filter
    {
        //NOTE: This is a low pass filter and therefore large sample widths will make the signal lose responsivity
        //Lag time = sampleWidth / sampleRate
        public MovingAverage(int sampleWidth_)
        {
            sampleWidth = sampleWidth_;
            
            data = new Queue<float>(sampleWidth);
            InitializeQueue(data, sampleWidth);

            sum = 0;
            return;
        }


        public int SampleWidth { get { return sampleWidth; } }


        public SensorLib.Filters.BidirectionalFilter AsBidirectional()
        {
            return new SensorLib.Filters.BidirectionalFilter(this);
        }

        public override bool IsRealTime { get { return true; } }

        public override void Reset()
        {
            InitializeQueue(data, sampleWidth);
            sum = 0;
            return;
        }

        public override float[] FilterData(float[] input)
        {
            if (input == null)
                return null;

            float[] output = new float[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                sum -= data.Dequeue();  //!!Doing the averaging this way can produce drift over a period of time!
                sum += input[i];
                data.Enqueue(input[i]);

                output[i] = sum / sampleWidth;
            }

            return output;
        }

        static void InitializeQueue(Queue<float> queue, int sampleWidth)
        {
            queue.Clear();
            for(int i=0; i<sampleWidth; i++)
                queue.Enqueue(0.0f);
        }


        readonly int sampleWidth;
        readonly Queue<float> data;
        float sum = 0;
    }
}
