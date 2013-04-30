using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;

using SensorLib.Sensors;


namespace SensorLib.Sensors
{

    /// <summary>
    /// An abstract class to help create devices that only creates sensors of a single data type.
    /// This device will do all the necessary notifications of the device and storage of sensors.
    /// </summary>
    /// <typeparam name="DeviceInfoType"></typeparam>
    /// <typeparam name="DataType"></typeparam>
    public abstract class Device<DeviceInfoType, DataType> : IDevice where DataType : new()
    {

        public Device()
        {
            sensors = new List<Sensor<DataType>>();

            isDisposed = false;
            return;
        }


        public void Dispose()
        {

            if (isDisposed)
                return;

            isDisposed = true;

            lock (sensors)
            {
                //Propagate the disposal to all the sensors
                while (sensors.Count > 0)
                {
                    sensors.First().Dispose();
                }
            }


            //Dispose of the device
            DeviceDispose();


            if (Disconnected != null)
                Disconnected(this);

            return;
        }

        
        /// <summary>
        /// Is fired whenever device becomes disconnected (regardless of the reason).
        /// </summary>
        public event Action<IDevice> Disconnected;

        /// <summary>
        /// Is fired when device was forced to be disconnected because of some error.
        /// </summary>
        public event Action<IDevice> Dropped;

        public abstract DeviceInfoType Info { get; }


        protected void Drop()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            lock (sensors)
            {
                //Propagate the disposal to all the sensors
                while (sensors.Count > 0)
                {
                    sensors.First().Drop();
                }
            }


            //Dispose of the device
            DeviceDispose();


            if (Dropped != null)
                Dropped(this);

            if (Disconnected != null)
                Disconnected(this);

            return;
        }


        protected void CreateSensor(Sensor<DataType> sensor)
        {
            lock (sensors)
            {
                sensors.Add(sensor);
            }
            sensor.Removed += new SensorEventHandler(RemoveSensor);
            sensor.Dropped += new SensorEventHandler(RemoveSensor);

            bool firstSensorAdded;
            lock (sensors)
            {
                firstSensorAdded = (sensors.Count == 1);
            }

            if (firstSensorAdded)
            {
                DeviceFirstSensorAdded();
            }
            

            return;
        }


        void RemoveSensor(ISensor removedSensor)
        {
            Sensor<DataType> sensor = (Sensor<DataType>)removedSensor;

            
            DeviceRemoveSensor(sensor);

            lock (sensors)
            {
                sensors.Remove(sensor);
            }

            sensor.Removed -= RemoveSensor;
            sensor.Dropped -= RemoveSensor;

            bool lastSensorRemoved;
            lock (sensors)
            {
                lastSensorRemoved = (sensors.Count == 0);
            }

            if (lastSensorRemoved)
            {
                DeviceLastSensorRemoved();
            }

            return;
        }



        protected void ReceivedData(DataType[] data)
        {
            // Create a copy of the sensors list so that we don't have to have the list locked while iterating (this will cut down on the chance of deadlock)
            List<Sensor<DataType>> sensorsCpy;
            lock (sensors)
            {
                sensorsCpy = new List<Sensor<DataType>>(sensors);
            }

            foreach (Sensor<DataType> sensor in sensorsCpy)
            {
                DeviceReceivedData(sensor, data);
            }
        
            return;
        }


        protected abstract void DeviceDispose();

        protected abstract void DeviceAddSensor(Sensor<DataType> sensor);
        protected abstract void DeviceRemoveSensor(Sensor<DataType> sensor);

        protected abstract void DeviceFirstSensorAdded();
        protected abstract void DeviceLastSensorRemoved();
        
        protected abstract void DeviceReceivedData(Sensor<DataType> sensor, DataType[] data);


        protected ReadOnlyCollection<Sensor<DataType>> Sensors { get { return sensors.AsReadOnly(); } }


        // Overriding Methods

        public override bool Equals(object obj)
        {
            Device<DeviceInfoType, DataType> a1 = obj as Device<DeviceInfoType, DataType>;
            if (a1 == null)
                return false;

            return Info.Equals(a1.Info);
        }

        public override int GetHashCode()
        {
            return Info.GetHashCode();
        }



        // Private Variables

        readonly List<Sensor<DataType>> sensors;

        bool isDisposed;

    }
}
