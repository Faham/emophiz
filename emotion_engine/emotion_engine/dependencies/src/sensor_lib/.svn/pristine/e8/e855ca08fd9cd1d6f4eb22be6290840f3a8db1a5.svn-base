using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors
{
    //Extends the Sensor interface by adding the concept of data
    public interface ISensor<T> : ISensor
    {
        /// <summary>
        /// Specifies whether the data will be queued or discarded.
        /// </summary>
        bool QueueData { get; }

        /// <summary>
        /// Returns the last processed value received from sensor.
        /// </summary>
        T CurrentValue { get; }

        /// <summary>
        /// Retrieves the data stored in the queue.
        /// </summary>
        /// <param name="maxSamples"> Maximum number of samples to receive. </param>
        /// <returns></returns>
        T[] GetData(int maxSamples);


        /// <summary>
        /// Is fired whenever the current value from the sensor changes.
        /// </summary>
        event CurrentValueChangedHandler<T> CurrentValueChanged;

        /// <summary>
        /// Is fired whenever there is data from the sensor available.
        /// </summary>
        event DataAvailableHandler<T> DataAvailable;


    }
}
