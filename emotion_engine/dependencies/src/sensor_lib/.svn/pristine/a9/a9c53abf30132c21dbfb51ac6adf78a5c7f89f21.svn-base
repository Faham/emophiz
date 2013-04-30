using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;
using SensorLib.Util;

namespace SensorLib.Mindset
{
    class MindsetInput : IInput<BrainData>
    {

        public MindsetInput()
        {
            dataQ = new TSQueue<BrainData>();
            return;
        }

        public void AddData(BrainData data)
        {
            dataQ.Enqueue(data);
            return;
        }

        public void AddData(BrainData[] data)
        {
            dataQ.Enqueue(data);
            return;
        }

        public void ClearInput()
        {
            dataQ.Clear();
            return;
        }

        public BrainData[] GetData(int maxSamples)
        {
            return dataQ.Dequeue(maxSamples);
        }

        //!!
        public float SamplesPerSecond { get { return 512; } }

        public void Disconnect()
        {
            if (Disconnected != null)
                Disconnected(this);

            return;
        }

        public void SignalExit()
        {
            dataQ.SignalExit();
            return;
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public event Action<IInput> Disconnected;

        readonly TSQueue<BrainData> dataQ;

    }
}
