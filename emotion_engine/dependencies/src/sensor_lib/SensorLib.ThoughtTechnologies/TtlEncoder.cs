using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Diagnostics;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

namespace SensorLib.ThoughtTechnologies
{

    public class TtlEncoder : Device<ITtlEncoderInfo, float>, ITtlEncoder
    {

        // Static Members //

        /// <summary>
        /// Note: This currently leaves all "connected" TtlEncoders useless and need to be reconnected.
        /// </summary>
        /// <returns></returns>
        public static ITtlEncoderInfo[] GetEncoders()
        {
            encoders = TtlLiveSession.Instance.GetEncoders();

            ITtlEncoderInfo[] encoderInfos = Array.ConvertAll<ITtlEncoder, ITtlEncoderInfo>(encoders, (x) => x.Info);
            return encoderInfos;
        }



        #region Connecting To Device

        public static ITtlEncoder Connect(ITtlEncoderInfo info)
        {
            return new TtlEncoder(info);
            //ITtlEncoder encoder = Array.Find(encoders, (x) => x.Info.SerialNumber == info.SerialNumber);
            //return encoder;
        }

        #endregion


        static ITtlEncoder[] encoders;




        // Constructors //

        internal TtlEncoder(ITtlEncoderInfo info)
        {
            this.info = info;
            return;
        }


        // Public Methods //


        public float GetBatteryPercentage()
        {
            return TtlLiveSession.Instance.GetBatteryPercentage(this);
        }

        public ITtlSensor CreateSensor(String name, SensorType type, Channel channel, bool queueData)
        {
            TtlSensor sensor = TtlLiveSession.Instance.CreateSensor(this, type, channel, name, queueData);
            base.CreateSensor(sensor);
            return sensor;
        }


        // Protected Methods //

        protected override void DeviceDispose() { return; }
        protected override void DeviceAddSensor(Sensors.Sensor<float> sensor) { return; }
        
        protected override void DeviceRemoveSensor(Sensors.Sensor<float> sensor)
        {
            TtlLiveSession.Instance.RemoveSensor((ITtlSensor)sensor);
            return;
        }

        protected override void DeviceFirstSensorAdded() { return; }
        protected override void DeviceLastSensorRemoved() { return; }

        protected override void DeviceReceivedData(Sensors.Sensor<float> sensor, float[] data)
        {
            ((QueueInput<float>)sensor.Input).AddData(data);
            return;
        }



        // Public Properties //

        public override ITtlEncoderInfo Info { get { return info; } }


        


        // Private Fields //

        readonly ITtlEncoderInfo info;

    }

}
