using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors
{
    /// <summary>
    /// Allows multiple processors to be linked together
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProcessorChain<T> : IProcessor<T>
    {
        public ProcessorChain(IProcessor<T>[] processors)
        {
            this.processors = processors;
            return;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IProcessor<T>> Processors { get { return processors.ToList().AsReadOnly(); } }


        public T[] ProcessData(T[] data)
        {
            return processors.Aggregate(data, (x, y) => y.ProcessData(x));
        }


        public float OutputSamplesPerSecond(float inputSamplesPerSecond)
        {
            return processors.Aggregate(inputSamplesPerSecond, (x,y) => y.OutputSamplesPerSecond(x));
        }


        readonly IProcessor<T>[] processors;
    }
}
