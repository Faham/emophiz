using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using TTLLiveCtrlLib;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

namespace SensorLib.ThoughtTechnologies
{

    using Timer = System.Windows.Forms.Timer;

    //Thought Technology Sensors

    //TODO: When the encoder is powered down, the samples available for all connected sensors always returns zero.  We can use a counter to determine that after a certain amount of time the encoder must be powered off!
    //      Should have a check to make sure that we are reading the sensor data faster than the maximum time specified (specified when connecting to the encoder)

    //NOTE: For simplicity's sake, all methods in this class should use the session thread except for the constructor.
    //      This makes synchronization easier (public methods and event handlers just need to switch to session thread) and it also makes the class thread-safe!

    //This class makes sure that we are always doing fast reads by reading with the same thread we created the COM object on
    //This class also makes sure that we never block the thread doing all the reading (the blocking seems to mess up the data stream (all zeros))
    //This class also ensures the uninitialization of the COM environment
    class TtlLiveSession : IDisposable
    {

        // Singleton Pattern //
        
        public static TtlLiveSession Instance
        {
            get
            {
                if(instance == null)
                    instance = new TtlLiveSession();
                return instance;
            }
        }

        static TtlLiveSession instance = null;

        //  //

        
        private TtlLiveSession()
        {

            channelToChannelHandle = new Dictionary<Channel, int>();
            channelHandleToSensor = new Dictionary<int, List<TtlSensor>>();
            sensorsCreated = 0;
            sensorsStarted = 0;

            applicationContext = new ApplicationContext();
            
            //Create thread
            sessionThread = new Thread(SessionThreadStart);
            sessionThread.IsBackground = true;
            sessionThread.SetApartmentState(ApartmentState.STA);

            //Start thread
            bool noTimeout;
            sessionThreadInitException = null;
            Monitor.Enter(sessionThread);
            {
                sessionThread.Start();
                noTimeout = Monitor.Wait(sessionThread, 5000);
            }
            Monitor.Exit(sessionThread);


            //Check for initialization error
            if (sessionThreadInitException != null)
            {
                throw sessionThreadInitException;
            }
            else if (!noTimeout)
            {
                throw new Exception("Initialization timed out!");
            }


            return;
        }

        

        public void Dispose()
        {
            //Exit message pump
            applicationContext.ExitThread();

            //Wait for thread to exit
            sessionThread.Join();
            
            return;
        }



        public float GetBatteryPercentage(ITtlEncoder encoder)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                return (float)sessionThreadControl.Invoke(new Func<ITtlEncoder, float>(GetBatteryPercentage), encoder);
            }

            if (encoder == null)
                return 0.0f;

            //Get percentage
            float percentage;
            try
            {
                TTLEncoder ttlEncoder = GetEncoder(encoder.Info);
                percentage = session.BatteryLevelPct[ttlEncoder.HND];
            }
            catch (KeyNotFoundException)
            {
                percentage = 0.0f;
            }

            return percentage;
        }
        


        
        /// <summary>
        /// Finds all the encoders connected to the computer.  WARNING: Removes all channels from encoders that are already connected!
        /// </summary>
        /// <returns></returns>
        public TtlEncoder[] GetEncoders()
        {
            if(sessionThreadControl.InvokeRequired)
            {
                return (TtlEncoder[])sessionThreadControl.Invoke(new Func<TtlEncoder[]>(GetEncoders));
            }

            
            //Find encoders
            TTLEncoder[] newEncoders = FindEncoders();

            //Get encoder's serial numbers because they will never change
            int encoderCount = newEncoders.Length;
            TtlEncoder[] encoders = new TtlEncoder[encoderCount];
            for (int i = 0; i < encoderCount; i++)
            {
                TtlEncoderInfo encoderInfo = new TtlEncoderInfo();
                encoderInfo.SerialNumber = newEncoders[i].SerialNumber;
                encoderInfo.ModelName = newEncoders[i].ModelName;
                encoderInfo.ModelType = newEncoders[i].ModelType.ToString();
                encoderInfo.FirmwareVersion = newEncoders[i].FirmwareVersion;
                encoderInfo.HardwareVersion = newEncoders[i].HardwareVersion;
                encoderInfo.ProtocolName = newEncoders[i].ProtocolName;
                encoderInfo.ProtocolType = newEncoders[i].ProtocolType.ToString();
                
                encoders[i] = new TtlEncoder(encoderInfo);
            }

            return encoders;
        }


        /*
        public void StartChannels()
        {
            session.StartChannels();
            channelsStarted = true;
            return;
        }

        public void StopChannels()
        {
            channelsStarted = false;
            session.StopChannels();
            return;
        }
        */


        
        // Note: We have to make sure the sensor is created on the calling thread so that events are fired on the calling thread.
        public TtlSensor CreateSensor(ITtlEncoder encoder, SensorType type, Channel channel, String name, bool queueData)
        {

            int channelHandle = GetChannelHandle(encoder, channel);

            float samplesPerSecond = GetSampleRate(channelHandle);

            TtlSensor sensor = CreateTtlSensor(type, name, channel, samplesPerSecond, queueData);
            AddSensor(channelHandle, sensor);

            sensorsCreated++;

            sensor.Started += new SensorEventHandler(sensor_Started);
            sensor.Stopped += new SensorEventHandler(sensor_Stopped);
            return sensor;
        }

        

        public void RemoveSensor(ITtlSensor sensor)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                sessionThreadControl.Invoke(new Action<ITtlSensor>(RemoveSensor), sensor);
                return;
            }
            
            // Get channel handle
            int channelHandle = channelToChannelHandle[sensor.Channel];
            
            // Get sensor list for the channel
            List<TtlSensor> sensors = channelHandleToSensor[channelHandle];

            // Remove sensor
            bool ret = sensors.Remove((TtlSensor)sensor);
            Debug.Assert(ret);

            sensorsCreated--;

            // Remove channel if no sensors left
            if (sensorsCreated == 0)
            {
                // Drop all channels (we can't drop the channels individually because the TTLLive SDK is broken)
                session.DropChannels();
                channelToChannelHandle.Clear();
                channelHandleToSensor.Clear();
            }

            return;
        }


        void sensor_Started(ISensor sensor)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                sessionThreadControl.Invoke(new Action<ISensor>(sensor_Started), sensor);
                return;
            }

            //TtlSensor ttlSensor = (TtlSensor)sensor;
            //int channelHandle = channelToChannelHandle[ttlSensor.Channel];
            //session.set_ChannelActive(channelHandle, 

            sensorsStarted++;

            if (sensorsStarted > 0)
            {
                session.StartChannels();
                dataTimer.Start();
            }

            return;
        }

        void sensor_Stopped(ISensor sensor)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                sessionThreadControl.Invoke(new Action<ISensor>(sensor_Stopped), sensor);
                return;
            }

            //TtlSensor ttlSensor = (TtlSensor)sensor;
            //int channelHandle = channelToChannelHandle[ttlSensor.Channel];
            //session.set_ChannelActive(channelHandle, 

            sensorsStarted--;

            if (sensorsStarted == 0)
            {
                session.StopChannels();
                dataTimer.Stop();
            }

            return;
        }



        
        void SessionThreadStart()
        {
            Monitor.Enter(sessionThread);
            {
                try
                {
                    sessionThreadControl = new Control();
                    sessionThreadControl.CreateControl();

                    session = TtlLiveCom.Get();

                    dataTimer = new Timer();
                    dataTimer.Interval = 100;
                    dataTimer.Tick += new EventHandler(dataTimer_Tick);

                    notificationTimer = new Timer();
                    notificationTimer.Interval = 100;
                    notificationTimer.Tick += new EventHandler(notificationTimer_Tick);
                    notificationTimer.Start();
                }
                catch (Exception ex)
                {
                    sessionThreadInitException = ex;
                    TtlLiveCom.Dispose();
                }
                Monitor.Pulse(sessionThread);
            }
            Monitor.Exit(sessionThread);

            //Create a message pump for this thread
            Application.Run(applicationContext);
            

            //Dispose of all stuff on this thread
            dataTimer.Stop();
            dataTimer.Dispose();

            notificationTimer.Stop();
            notificationTimer.Dispose();

            TtlLiveCom.Dispose();
            sessionThreadControl.Dispose();

            return;
        }




        int GetChannelHandle(ITtlEncoder encoder, Channel channel)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                Func<ITtlEncoder, Channel, int> func = new Func<ITtlEncoder, Channel, int>(GetChannelHandle);
                return (int)sessionThreadControl.Invoke(func, encoder, channel);
            }

            // Check if we already have this channel
            int channelHandle;
            if (channelToChannelHandle.TryGetValue(channel, out channelHandle))
                return channelHandle;


            //Find the encoder
            TTLEncoder ttlEncoder = GetEncoder(encoder.Info);


            //Get the first available channel handle on the encoder
            channelHandle = -1;  //passing in -1 means AddChannel() will just return the first available channel
            ttlEncoder.AddChannel((TTLAPI_CHANNELS)channel, ref channelHandle);
            
            // Add channel
            channelToChannelHandle.Add(channel, channelHandle);
            channelHandleToSensor.Add(channelHandle, new List<TtlSensor>());

            return channelHandle;
        }

        float GetSampleRate(int channelHandle)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                Func<int, float> func = new Func<int, float>(GetSampleRate);
                return (float)sessionThreadControl.Invoke(func, channelHandle);
            }

            return session.get_NominalSampleRate(channelHandle);
        }

        void AddSensor(int channelHandle, TtlSensor sensor)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                Action<int, TtlSensor> func = new Action<int, TtlSensor>(AddSensor);
                sessionThreadControl.Invoke(func, channelHandle, sensor);
                return;
            }

            channelHandleToSensor[channelHandle].Add(sensor);
            return;
        }
        

        void notificationTimer_Tick(object sender, EventArgs e)
        {
            //Gather data from each channel
            int channelHandle = session.GetFirstChannelHND();
            while (channelHandle >= 0)
            {
                int notification = session.Notification[channelHandle];
                if(notification != 0)
                    Console.WriteLine("Notification: " + notification);
                channelHandle = session.GetNextChannelHND();
            }
            return;
        }



        TTLEncoder GetEncoder(ITtlEncoderInfo encoderInfo)
        {
            TTLEncoder encoder = session.GetFirstEncoder();
            while (encoder.SerialNumber != encoderInfo.SerialNumber && encoder != null)
            {
                encoder = session.GetNextEncoder();
            }

            if (encoder == null)
                throw new KeyNotFoundException();

            return encoder;
        }


        TTLEncoder[] FindEncoders()
        {
            //Open connections to find all encoders connected to computer
            //NOTE: If we get an exception "COM object that has been separated from its underlying RCW cannot be used." it might be thrown because session is being called on a different thread that has a different thread mode (STA/MTA)
            //      Also happens when you dispose the liveSessionTtl and then try calling this.
            int scanned = 0;
            int detected = 0;
            session.OpenConnections(
                        (int)TTLLiveCtrlLib.TTLAPI_OPENCONNECTIONS_CMD_BITS.TTLAPI_OCCMD_AUTODETECT  //This re-enumerates all encoder handles!
                    , 1000          //a value, in milliseconds, specifying the maximum expected interval between reads on any channel. This affects the amount of memory the API reserves as buffer space for each channel.
                    , ref scanned
                    , ref detected
                    );

            //Put encoders into a nice array
            int encoderCount = session.EncoderCount;
            TTLEncoder[] encoders = new TTLEncoder[encoderCount];
            for (int i = 0; i < encoderCount; i++)
            {
                encoders[i] = session.Encoder[i];
            }

            return encoders;
        }


        TtlSensor CreateTtlSensor(SensorType sensorType, String name, Channel channel, float samplesPerSecond, bool queueData)
        {

            TtlSensor sensor;
            switch (sensorType)
            {

                //case SensorType.Brain:
                //    sensor = new BrainSensor(new BrainSensor.Processor(), name, channel, sampleRate, queueData);
                //    break;

                case SensorType.Bvp:
                    sensor = new BvpSensor(new BvpProcessor(), name, channel, samplesPerSecond, queueData);
                    break;

                case SensorType.Gsr:
                    sensor = new GsrSensor(new GsrProcessor(), name, channel, samplesPerSecond, queueData);
                    break;

                case SensorType.Heart:
                    sensor = new HeartSensor(new HeartProcessor(), name, channel, samplesPerSecond, queueData);
                    break;

                case SensorType.Muscle:
                    sensor = new MuscleSensor(new MuscleProcessor(), name, channel, samplesPerSecond, queueData);
                    break;

                case SensorType.Strain:
                    sensor = new StrainSensor(new StrainProcessor(), name, channel, samplesPerSecond, queueData);
                    break;

                case SensorType.Temperature:
                    sensor = new TempSensor(new TempProcessor(), name, channel, samplesPerSecond, queueData);
                    break;

                case SensorType.Raw:
                    sensor = new RawSensor(name, channel, samplesPerSecond, queueData);
                    break;

                default:
                    sensor = null;
                    Debug.Assert(false);
                    break;
            }


            return sensor;
        }
        
        void dataTimer_Tick(object sender, EventArgs e)
        {
            //Gather data from each channel
            int channelHandle = session.GetFirstChannelHND();

            while (channelHandle >= 0)
            {
                int samplesAvailable = session.SamplesAvailable[channelHandle];
                while (samplesAvailable > 0)
                {
                    int samples = (samplesAvailable > MAX_SAMPLES) ? MAX_SAMPLES : samplesAvailable;
                    float[] data = (float[])session.ReadChannelDataVT(channelHandle, samples);

                    channelHandleToSensor[channelHandle].Where(x => x.IsStarted).ToList().ForEach(x => ((QueueInput<float>)x.Input).AddData(data));

                    samplesAvailable -= data.Length;
                }

                channelHandle = session.GetNextChannelHND();
            }


            /*
            // Gather data from each channel with sensors
            foreach (KeyValuePair<int, List<TtlSensor>> pair in channelHandleToSensor)
            {
                if (pair.Value.Count == 0)
                    continue;

                int channelHandle = pair.Key;

                int samplesAvailable = session.SamplesAvailable[channelHandle];
                while (samplesAvailable > 0)
                {
                    int samples = (samplesAvailable > MAX_SAMPLES) ? MAX_SAMPLES : samplesAvailable;
                    float[] data = (float[])session.ReadChannelDataVT(channelHandle, samples);

                    channelHandleToSensor[channelHandle].ForEach(x => ((QueueInput<float>)x.Input).AddData(data));

                    samplesAvailable -= data.Length;
                }
            }
            */
            
            
            return;
        }


        


        


        const int MAX_SAMPLES = 255;

        readonly Dictionary<Channel, int> channelToChannelHandle;
        readonly Dictionary<int, List<TtlSensor>> channelHandleToSensor;

        //Threading stuff
        readonly Thread sessionThread;
        Control sessionThreadControl;  //is used so we can Invoke() it and call stuff on the session thread  //!!(use ISynchronizeInvoke instead?)
        Exception sessionThreadInitException;
        readonly ApplicationContext applicationContext;  //Allows us to exit the message pump on the session thread (usually it just listens to the Closed() event on the main form)

        ITTLLive2 session;
        Timer dataTimer;
        Timer notificationTimer;
        

        int sensorsStarted;
        int sensorsCreated;

        
    }
}
