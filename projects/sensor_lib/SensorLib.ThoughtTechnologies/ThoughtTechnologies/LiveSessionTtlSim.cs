using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace SensorLib
{
    /*
    class LiveSessionTtlSim : ILiveSessionTtl
    {

        public LiveSessionTtlSim()
        {
            dictionary = new Dictionary<Channel, List<TtlChannelSim>>();
            return;
        }
        
        //!!These don't make fake monitors stop triggering
        public void StartChannels()
        {
            lock (dictionary)
            {
                foreach (List<TtlChannelSim> sensors in dictionary.Values)
                {
                    foreach (ISensor sensor in sensors)
                    {
                        sensor.Start();
                    }
                }
                //channelsStarted = true;
            }
            return;
        }

        public void StopChannels()
        {
            lock (dictionary)
            {
                foreach (List<TtlChannelSim> sensors in dictionary.Values)
                {
                    foreach (ISensor sensor in sensors)
                    {
                        sensor.Stop();
                    }
                }
                //channelsStarted = false;
            }
            return;
        }

        public TtlEncoder[] GetEncoders()
        {
            return new TtlEncoder[0];
        }

        public float BatteryPercentage { get { return 0f; } }


        /////////////////////
        // Factory Methods //
        /////////////////////


        public ISensor CreateSensor(SensorType type, Channel channel)
        {
            return CreateSensor(String.Format("{0} Sim ({1})", type, channel), type, channel);
        }

        public ISensor CreateSensor(String sensorName_, SensorType type, Channel channel)
        {
            return CreateSensor(sensorName_, type, channel, false);
        }

        public ISensor CreateSensor(String sensorName_, SensorType type, Channel channel, bool dataAvailable)
        {
            ISensor sensor;
            ISensorSim sensorSim;
            switch (type)
            {
                case SensorType.Brain:
                {
                    BrainSensorSim sensorClass = new BrainSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                case SensorType.Bvp:
                {
                    BvpSensorSim sensorClass = new BvpSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                case SensorType.Gsr:
                {
                    GsrSensorSim sensorClass = new GsrSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                case SensorType.Heart:
                {
                    HeartSensorSim sensorClass = new HeartSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                case SensorType.Muscle:
                {
                    MuscleSensorSim sensorClass = new MuscleSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                case SensorType.Strain:
                {
                    StrainSensorSim sensorClass = new StrainSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                case SensorType.Temperature:
                {
                    TempSensorSim sensorClass = new TempSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                case SensorType.Raw:
                {
                    RawSensorSim sensorClass = new RawSensorSim(sensorName_);
                    sensor = (ISensor)sensorClass;
                    sensorSim = (ISensorSim)sensorClass;
                    break;
                }

                default:
                {
                    sensor = null;
                    sensorSim = null;
                    Debug.Assert(false);
                    break;
                }
            }


            AddChannel(channel, sensorSim);
            return sensor;
        }


        public ITempSensor CreateTempSensor(Channel channel)
        {
            return CreateTempSensor(String.Format("Temp Sim ({0})", channel), channel);
        }

        public ITempSensor CreateTempSensor(String sensorName_, Channel channel)
        {
            return CreateTempSensor(sensorName_, channel, false);
        }

        public ITempSensor CreateTempSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            TempSensorSim sensor = new TempSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        public IBvpSensor CreateBvpSensor(Channel channel)
        {
            return CreateBvpSensor(String.Format("BVP Sim (BPM) ({0})", channel), channel);
        }

        public IBvpSensor CreateBvpSensor(String sensorName_, Channel channel)
        {
            return CreateBvpSensor(sensorName_, channel, false);
        }

        public IBvpSensor CreateBvpSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            BvpSensorSim sensor = new BvpSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        public IGsrSensor CreateGsrSensor(Channel channel)
        {
            return CreateGsrSensor(String.Format("GSR Sim ({0})", channel), channel);
        }

        public IGsrSensor CreateGsrSensor(String sensorName_, Channel channel)
        {
            return CreateGsrSensor(sensorName_, channel, false);
        }

        public IGsrSensor CreateGsrSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            GsrSensorSim sensor = new GsrSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        public IStrainSensor CreateStrainSensor(Channel channel)
        {
            return CreateStrainSensor(String.Format("Strain Sim ({0})", channel), channel);
        }

        public IStrainSensor CreateStrainSensor(String sensorName_, Channel channel)
        {
            return CreateStrainSensor(sensorName_, channel, false);
        }

        public IStrainSensor CreateStrainSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            StrainSensorSim sensor = new StrainSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        public IMuscleSensor CreateMuscleSensor(Channel channel)
        {
            return CreateMuscleSensor(String.Format("Muscle Sim ({0})", channel), channel);
        }

        public IMuscleSensor CreateMuscleSensor(String sensorName_, Channel channel)
        {
            return CreateMuscleSensor(sensorName_, channel, false);
        }

        public IMuscleSensor CreateMuscleSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            MuscleSensorSim sensor = new MuscleSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        public IHeartSensor CreateHeartSensor(Channel channel)
        {
            return CreateHeartSensor(String.Format("Heart Sim (BPM) ({0})", channel), channel);
        }

        public IHeartSensor CreateHeartSensor(String sensorName_, Channel channel)
        {
            return CreateHeartSensor(sensorName_, channel, false);
        }

        public IHeartSensor CreateHeartSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            HeartSensorSim sensor = new HeartSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        public IBrainSensor CreateBrainSensor(Channel channel)
        {
            return CreateBrainSensor(channel);
        }

        public IBrainSensor CreateBrainSensor(String sensorName_, Channel channel)
        {
            return CreateBrainSensor(sensorName_, channel, false);
        }

        public IBrainSensor CreateBrainSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            BrainSensorSim sensor = new BrainSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        /*
        public IAudioSensor CreateAudioSensor(Channel channel)
        {
            return CreateAudioSensor(channel, false);
        }

        public IAudioSensor CreateAudioSensor(String sensorName_, Channel channel, bool dataAvailable)
        {
            return new AudioSensorSim(sensorName_);
        }

        public IEyeTracker[] EyeTrackers { get { return null; } }  //!!null!

        public IEyeSensor CreateEyeSensor(IEyeTracker eyeTracker)
        {
            return null; //!!null!
        }
        *//*

        public IRawSensor CreateRawSensor(Channel channel)
        {
            return CreateRawSensor(String.Format("Raw Sim ({0})", channel), channel);
        }

        public IRawSensor CreateRawSensor(String sensorName_, Channel channel)
        {
            RawSensorSim sensor = new RawSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }

        public IRawSensor CreateRawSensor(String sensorName_, Channel channel, bool queueData_)
        {
            RawSensorSim sensor = new RawSensorSim(sensorName_);

            AddChannel(channel, sensor);
            return sensor;
        }


        public void Finish()
        {
            return;
        }


        void AddChannel(Channel channel, /*ISensorSim*//*ISensor sensor)
        {

            lock (dictionary)
            {
                //Check if channel is already in use
                if (dictionary.ContainsKey(channel))
                {
                    List<ISensorSim> sensors;
                    dictionary.TryGetValue(channel, out sensors);
                    sensors.Add(sensor);
                    return;
                }
                else
                {
                    dictionary.Add(channel, new List</*ISensorSim*//*ISensor> { sensor } );
                }
                
                return;
            }

        }

        //bool channelsStarted = false;
        Dictionary<Channel, List</*ISensorSim*//*TtlChannelSim>> dictionary;

    }
        */
}
