using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Diagnostics;


using SensorLib.Sensors.Inputs;


namespace SensorLib.Sensors
{
    //A basic sensor only starts and stops
    public class Sensor : ISensor, IDisposable
    {
        
        public Sensor(String name, IProcessor processor, IInput input)
        {
            this.name = name;
            this.input = input;
            this.processor = processor;


            IsStarted = false;
            IsDisposed = false;

            return;
        }


        public DateTime StartTime { get { return startTime; } }
        public String Name { get { return name; } }
        public bool IsStarted { get; private set; }

        public bool IsDisposed { get; private set; }

        public bool HasConstantSampleRate { get { return SamplesPerSecond != 0; } }
        public float SamplesPerSecond { get { return processor.OutputSamplesPerSecond(input.SamplesPerSecond); } }
        //public abstract ManufacturerType Manufacturer { get; }
        //public abstract SensorType SensorType { get; }


        public event SensorEventHandler Started;
        public event SensorEventHandler Stopped;
        public event SensorEventHandler Dropped;
        public event SensorEventHandler Removed;

        
        
        public virtual void Dispose()
        {
            if(IsDisposed)
                return;

            Stop();

            IsDisposed = true;

            // Fire events
            if (Removed != null)
                Removed(this);
            return;
        }

        //Should only be dropped by the device!
        public virtual void Drop()
        {
            if (IsDisposed)
                return;


            Stop();

            IsDisposed = true;

            // Fire events
            if (Dropped != null)
                Dropped(this);

            if (Removed != null)
                Removed(this);
            return;
        }

        
        public void Start()
        {
            Start(DateTime.Now);
            return;
        }


        public virtual void Start(DateTime startTime)
        {
            if (IsStarted)
                return;

            lock(isStartedLock)
            {
                this.startTime = startTime;
                IsStarted = true;
                input.Start();
                OnStart(startTime);
            }

            // Fire events
            if (Started != null)
                Started(this);

            return;
        }

        public virtual void Stop()
        {
            if (!IsStarted)
                return;

            lock (isStartedLock)
            {
                IsStarted = false;
                input.Stop();
                OnStop();
            }
            
            // Fire events
            if(Stopped != null)
                Stopped(this);

            return;
        }

        /// <summary>
        /// Is called when the sensor starts.  Is wrapped around a lock.
        /// </summary>
        /// <param name="startTime"></param>
        protected virtual void OnStart(DateTime startTime) { return; }

        /// <summary>
        /// Is called when the sensor stops.  Is wrapped around a lock.
        /// </summary>
        protected virtual void OnStop() { return; }



        readonly String name;
        readonly IInput input;
        readonly IProcessor processor;

        DateTime startTime;

        protected readonly object isStartedLock = new object();
    }
}
