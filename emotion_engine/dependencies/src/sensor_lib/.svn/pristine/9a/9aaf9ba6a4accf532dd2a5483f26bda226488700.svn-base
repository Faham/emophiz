using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors
{
    //Basic interface to a sensor
    public interface ISensor : IDisposable
    {
        /// <summary>
        /// Name of the sensor.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Timestamps are all relative to this time.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Data rate in samples per second.  A value of zero means the sample rate is variable.
        /// </summary>
        float SamplesPerSecond { get; }

        /// <summary>
        /// Returns whether the output sample rate is constant.
        /// </summary>
        bool HasConstantSampleRate { get; }

        /// <summary>
        /// Whether the sensor is started or not.
        /// </summary>
        bool IsStarted { get; }


        /// <summary>
        /// Starts the sensor.
        /// </summary>
        void Start();

        /// <summary>
        /// Starts the sensor with timestamps relative to the specified start time.
        /// </summary>
        void Start(DateTime startTime);

        /// <summary>
        /// Stops the sensor.
        /// </summary>
        void Stop();



        event SensorEventHandler Started;
        event SensorEventHandler Stopped;
        
        /// <summary>
        /// Is fired when device or sensor is disposed.
        /// </summary>
        event SensorEventHandler Removed;

        /// <summary>
        /// Is fired when device or sensor was forced to be removed due to error or other disconnection.
        /// </summary>
        event SensorEventHandler Dropped;

        /// <summary>
        /// Is fired when an error occurs causing the EEG to drop.
        /// </summary>
        //event Action<ISensor, String> Error;


        //ManufacturerType Manufacturer { get; }
        //SensorType SensorType { get; }

        
    }
}
