using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace SensorLib.Sensors
{

    /// <summary>
    /// The goal of SensorLib is to make a framework where different types of sensors (ex. EEG, GSR, etc.) can be accessed with a similiar interface and are manufacturer agnostic.
    /// </summary>
    public class LiveSession : ILiveSession
    {

        public static ILiveSession CreateInstance()
        {
            return CreateInstance(false);
        }

        public static ILiveSession CreateInstance(bool simulate_)
        {
            return new LiveSession(simulate_);
        }

        public static void ReleaseInstance(ILiveSession liveSession_)
        {
            ((LiveSession)liveSession_).Release();  //!!Need to keep instance! (singleton?)
            return;
        }





        //////////////////////
        // Instance Methods //
        //////////////////////

        protected LiveSession(bool simulate_)
        {
            //Check to see if there is any synchronization context on this thread
            //hasSynchContext = System.Threading.SynchronizationContext.Current != null;

            //simulate = simulate_;
            EyeTracker.EyeTrackerAdded += new EyeTrackerEvent(EyeTracker_EyeTrackerAdded);
            EyeTracker.EyeTrackerRemoved += new EyeTrackerEvent(EyeTracker_EyeTrackerRemoved);
            return;
        }


        void Release()
        {
            /*
            if (ttlSession != null)
            {
                ttlSession.Finish();
                LiveSessionTtl.ReleaseInstance();
            }
             */
            return;
        }


        public float GetBatteryPercentage(ITtlEncoder encoder)
        {
            return LiveSessionTtl.CreateInstance().GetBatteryPercentage(encoder);
        }

        
        public void StartAllSensors()
        {
            /*
            if(ttlSession != null)
                ttlSession.StartAllChannels();

            foreach (EyeSensor eyeSensor in eyeSensors)
                eyeSensor.Start();
            */
            Logger.StartLogging();

            //sensorsStarted = true;
            return;
        }

        
        public void StopAllSensors()
        {
            /*
            if (ttlSession != null)
                ttlSession.StopAllChannels();

            foreach (EyeSensor eyeSensor in eyeSensors)
                eyeSensor.Stop();
            */
            Logger.StopLogging();

            //sensorsStarted = false;
            return;
        }



        public void RemoveSensor(ISensor sensor)
        {
            switch(sensor.Manufacturer)
            {
                case ManufacturerType.DirectX:
                    ((Sensor)sensor).Dispose();
                    break;

                case ManufacturerType.Tobii:
                    ((Sensor)sensor).Dispose();
                    break;

                case ManufacturerType.Ttl:
                    LiveSessionTtl.CreateInstance().RemoveSensor((ITtlSensor)sensor);
                    ((Sensor)sensor).Dispose();
                    break;

                case ManufacturerType.Neurosky:
                    mindset.RemoveInput((MindsetInput)(((Sensor<BrainData>)sensor).Input));
                    ((Sensor)sensor).Dispose();
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            if (SensorRemoved != null)
                SensorRemoved(sensor);

            return;
        }

        
        public event SensorEventHandler SensorDropped;
        public event SensorEventHandler SensorRemoved;

        public void SensorDroppedEvent(ISensor sensor)
        {
            SensorDropped(sensor);
            return;
        }



        void EyeTracker_EyeTrackerAdded(IEyeTracker eyeTracker)
        {
            if (EyeTrackerAdded != null)
                EyeTrackerAdded(eyeTracker);
        }

        void EyeTracker_EyeTrackerRemoved(IEyeTracker eyeTracker)
        {
            if (EyeTrackerRemoved != null)
                EyeTrackerRemoved(eyeTracker);
        }


        /////////////////////
        // DirectX Sensors //
        /////////////////////

            ///////////
            // Audio //
            ///////////

        public IAudioSensor CreateAudioSensor()
        {
            return CreateAudioSensor("Audio");
        }

        public IAudioSensor CreateAudioSensor(String sensorName_)
        {
            return CreateAudioSensor(sensorName_, false);
        }

        public IAudioSensor CreateAudioSensor(String sensorName_, bool queueData)
        {
            return new AudioSensor(new AudioSensor.Processor(), new Inputs.DirectXAudioInput(), sensorName_, queueData);
        }


        //////////////////////
        // Neurosky Sensors //
        //////////////////////

        public void ConnectMindset(String portName)
        {
            mindset = Mindset.Connect(portName);
            mindset.Disconnected += new Action<Mindset>(mindset_Disconnected);
            if (MindsetConnectedChanged != null)
                MindsetConnectedChanged(true);
            return;
        }


        public void DisconnectMindset()
        {
            Mindset.Disconnect(mindset);
            mindset = null;
            
            if (MindsetConnectedChanged != null)
                MindsetConnectedChanged(false);
            return;
        }

        void mindset_Disconnected(Mindset mindset)
        {
            mindset = null;

            if (MindsetConnectedChanged != null)
                MindsetConnectedChanged(false);

            return;
        }

        public bool MindsetConnected { get { return mindset != null; } }
        public event Action<bool> MindsetConnectedChanged;


        public IBrainSensor CreateBrainSensor(String sensorName_)
        {
            return new BrainSensor(mindset.CreateInput(), sensorName_, 0.0f, false);
        }


        public IBrainSensor CreateBrainSensor(String sensorName_, bool queueData)
        {
            return new BrainSensor(mindset.CreateInput(), sensorName_, 0.0f, queueData);
        }

        


        ////////////////////////////////
        // Eye Sensor (Tobii Sensors) //
        ////////////////////////////////

            /////////
            // Eye //
            /////////

        public IEyeTracker[] EyeTrackers { get { return EyeTracker.GetEyeTrackers().ToArray(); } }
        public event EyeTrackerEvent EyeTrackerAdded;
        public event EyeTrackerEvent EyeTrackerRemoved;

        public IEyeSensor CreateEyeSensor(IEyeTracker eyeTracker)
        {
            return CreateEyeSensor("Eye", eyeTracker, false);
        }

        public IEyeSensor CreateEyeSensor(String sensorName_, IEyeTracker eyeTracker)
        {
            return CreateEyeSensor(sensorName_, eyeTracker, false);
        }

        public IEyeSensor CreateEyeSensor(String sensorName_, IEyeTracker eyeTracker, bool queueData)
        {
            EyeSensor eyeSensor = new EyeSensor(new EyeSensor.Processor(), new EyeSensor.Input(eyeTracker), sensorName_, eyeTracker, queueData);
            eyeSensors.Add(eyeSensor);
            return eyeSensor;
        }

        static List<EyeSensor> eyeSensors = new List<EyeSensor>();  //!!Should not have to be static!


        ////////////////////////////////
        // Thought Technology Sensors //
        ////////////////////////////////

        public ITtlEncoder[] TtlEncoders { get { return LiveSessionTtl.CreateInstance().GetEncoders(); } }

        
        public ITtlSensor CreateTtlSensor(SensorType type, String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateTtlSensor(type, name, encoder, channel, false);
        }


        public ITtlSensor CreateTtlSensor(SensorType type, String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            return LiveSessionTtl.CreateInstance().CreateSensor(encoder, type, channel, name, queueData);
        }

        

        public ITempSensor CreateTempSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateTempSensor(name, encoder, channel, false);
        }

        public ITempSensor CreateTempSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Temperature, channel, name, queueData);
            return (TempSensor)sensor;
        }


        
        public IBvpSensor CreateBvpSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateBvpSensor(name, encoder, channel, false);
        }

        public IBvpSensor CreateBvpSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Bvp, channel, name, queueData);
            return (BvpSensor)sensor;
        }

        
        
        public IGsrSensor CreateGsrSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateGsrSensor(name, encoder, channel, false);
        }

        public IGsrSensor CreateGsrSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Gsr, channel, name, queueData);
            return (GsrSensor)sensor;
        }


        
        public IStrainSensor CreateStrainSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateStrainSensor(name, encoder, channel, false);
        }

        public IStrainSensor CreateStrainSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Strain, channel, name, queueData);
            return (StrainSensor)sensor;
        }



        public IMuscleSensor CreateMuscleSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateMuscleSensor(name, encoder, channel, false);
        }

        public IMuscleSensor CreateMuscleSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Muscle, channel, name, queueData);
            return (MuscleSensor)sensor;
        }



        public IHeartSensor CreateHeartSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateHeartSensor(name, encoder, channel, false);
        }

        public IHeartSensor CreateHeartSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Heart, channel, name, queueData);
            return (HeartSensor)sensor;
        }


        /*
        public IBrainSensor CreateBrainSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateBrainSensor(name, encoder, channel, false);
        }

        
        public IBrainSensor CreateBrainSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Brain, channel, name, queueData);
            return (BrainSensor)sensor;
        }
        */


        public IRawSensor CreateRawSensor(String name, ITtlEncoder encoder, Channel channel)
        {
            return CreateRawSensor(name, encoder, channel, false);
        }

        public IRawSensor CreateRawSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData)
        {
            TtlSensor sensor = LiveSessionTtl.CreateInstance().CreateSensor(encoder, SensorType.Raw, channel, name, queueData);
            return (RawSensor)sensor;
        }


        //public bool AllowsEvents { get { return hasSynchContext; } }  //We can only fire events if there is a synchronization context on the creation thread

        //public event EventHandler TtlEncoderAdded;
        //public event EventHandler TtlEncoderRemoved;


        //readonly bool simulate;
        //bool sensorsStarted = false;


        //readonly bool hasSynchContext;


        Mindset mindset = null;
    }
}
