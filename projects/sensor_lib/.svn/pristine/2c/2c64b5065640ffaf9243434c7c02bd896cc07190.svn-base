using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Util;

namespace SensorLib.Sensors.Inputs
{

    /// <summary>
    /// A basic queue to hold data.  Useful for holding data coming from a device.
    /// </summary>
    /// <typeparam name="T"> Type of data to hold. </typeparam>
    public class QueueInput<T> : IInput<T>
    {

        public QueueInput(float samplesPerSecond)
        {
            this.samplesPerSecond = samplesPerSecond;
            isStarted = false;
            dataQ = new TSQueue<T>();
            return;
        }


        public float SamplesPerSecond { get { return samplesPerSecond; } }
        public event Action<IInput> Disconnected;

        public T[] GetData(int maxSamples)
        {
            return dataQ.Dequeue(maxSamples);
        }

        
        public void Disconnect()
        {
            lock (dataQ)
            {
                dataQ.SignalExit();
            }

            if(Disconnected != null)
                Disconnected(this);

            return;
        }

        public void AddData(T[] data)
        {
            lock (dataQ)
            {
                if (!isStarted)
                    return;

                dataQ.Enqueue(data);
            }
            return;
        }

        public void Start()
        {
            lock (dataQ)
            {
                isStarted = true;
            }
            return;
        }

        public void Stop()
        {
            lock (dataQ)
            {
                isStarted = false;
                dataQ.Clear();
            }
            return;
        }

        readonly TSQueue<T> dataQ;
        readonly float samplesPerSecond;
        
        bool isStarted;
    }
}
