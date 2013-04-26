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

namespace SensorLib
{

    using Timer = System.Windows.Forms.Timer;

    //Thought Technology Sensors


    //NOTE: For simplicity sake, all methods in this class should use the session thread except for the constructor.
    //      This makes synchronization easier (public methods and event handlers just need to switch to session thread) and it also makes the class thread-safe!

    //This class makes sure that we are always doing fast reads by reading with the same thread we created the COM object on
    //This class also makes sure that we never block the thread doing all the reading (the blocking seems to mess up the data stream (all zeros))
    //This class also ensures the uninitialization of the COM environment
    partial class LiveSessionTtl : ILiveSessionTtl, IDisposable
    {

        public LiveSessionTtl()
        {

            channelHandleToSensor = new Dictionary<int, TtlSensor>();
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
                TTLEncoder ttlEncoder = GetEncoder(encoder);
                percentage = session.BatteryLevelPct[ttlEncoder.HND];
            }
            catch (KeyNotFoundException)
            {
                percentage = 0.0f;
            }

            return percentage;
        }
        
        
        /// <summary>
        /// Finds all the encoders connected to the computer.  Removes all channels from encoders that are already connected!
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
                TtlEncoder encoder = new TtlEncoder();
                encoder.SerialNumber = newEncoders[i].SerialNumber;
                encoder.ModelName = newEncoders[i].ModelName;
                encoder.ModelType = newEncoders[i].ModelType.ToString();
                encoder.FirmwareVersion = newEncoders[i].FirmwareVersion;
                encoder.HardwareVersion = newEncoders[i].HardwareVersion;
                encoder.ProtocolName = newEncoders[i].ProtocolName;
                encoder.ProtocolType = newEncoders[i].ProtocolType.ToString();
                
                encoders[i] = encoder;
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


        

        public TtlSensor CreateSensor(ITtlEncoder encoder, SensorType type, Channel channel, String name, bool queueData)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                Func<ITtlEncoder, SensorType, Channel, String, bool, TtlSensor> func = new Func<ITtlEncoder, SensorType, Channel, String, bool, TtlSensor>(CreateSensor);
                return (TtlSensor)sessionThreadControl.Invoke(func, encoder, type, channel, name, queueData);
            }


            //Find the encoder
            TTLEncoder ttlEncoder = GetEncoder(encoder);
            

            //Get the first available channel handle on the encoder
            int channelHandle = -1;  //passing in -1 means AddChannel() will just return the first available channel
            ttlEncoder.AddChannel((TTLAPI_CHANNELS)channel, ref channelHandle);

            float sampleRate = session.get_NominalSampleRate(channelHandle);

            TtlSensor sensor = CreateTtlSensor(type, name, channel, sampleRate, queueData);
            channelHandleToSensor.Add(channelHandle, sensor);

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

            int channelHandle = GetChannelHandle(sensor);
            session.DropChannel(channelHandle);
            return;
        }


        void sensor_Started(ISensor sensor)
        {
            if (sessionThreadControl.InvokeRequired)
            {
                sessionThreadControl.Invoke(new Action<ISensor>(sensor_Started), sensor);
                return;
            }

            TtlSensor ttlSensor = (TtlSensor)sensor;
            int channelHandle = GetChannelHandle(ttlSensor);
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

            TtlSensor ttlSensor = (TtlSensor)sensor;
            int channelHandle = GetChannelHandle(ttlSensor);
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

                    session = TtlLiveSingleton.Get();

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
                    TtlLiveSingleton.Dispose();
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

            TtlLiveSingleton.Dispose();
            sessionThreadControl.Dispose();

            return;
        }

        

        int GetChannelHandle(ITtlSensor sensor)
        {
            KeyValuePair<int, TtlSensor> pair = channelHandleToSensor.First((x) => x.Value == sensor);
            
            int channelHandle = pair.Key;
            return channelHandle;
        }

        

        void notificationTimer_Tick(object sender, EventArgs e)
        {
            //Gather data from each channel
            int channelHandle = session.GetFirstChannelHND();
            while (channelHandle >= 0)
            {
                int notification = session.Notification[channelHandle];
                Console.WriteLine("Notification: " + notification);
                channelHandle = session.GetNextChannelHND();
            }
            return;
        }



        TTLEncoder GetEncoder(ITtlEncoder encoderInfo)
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


        TtlSensor CreateTtlSensor(SensorType sensorType, String name, Channel channel, float sampleRate, bool queueData)
        {
            TtlSensor sensor;
            switch (sensorType)
            {

                //case SensorType.Brain:
                //    sensor = new BrainSensor(new BrainSensor.Processor(), name, channel, sampleRate, queueData);
                //    break;

                case SensorType.Bvp:
                    sensor = new BvpSensor(new BvpSensor.Processor(), name, channel, sampleRate, queueData);
                    break;

                case SensorType.Gsr:
                    sensor = new GsrSensor(new GsrSensor.Processor(), name, channel, sampleRate, queueData);
                    break;

                case SensorType.Heart:
                    sensor = new HeartSensor(new HeartSensor.Processor(), name, channel, sampleRate, queueData);
                    break;

                case SensorType.Muscle:
                    sensor = new MuscleSensor(new MuscleSensor.Processor(), name, channel, sampleRate, queueData);
                    break;

                case SensorType.Strain:
                    sensor = new StrainSensor(new StrainSensor.Processor(), name, channel, sampleRate, queueData);
                    break;

                case SensorType.Temperature:
                    sensor = new TempSensor(new TempSensor.Processor(), name, channel, sampleRate, queueData);
                    break;

                case SensorType.Raw:
                    sensor = new RawSensor(new RawSensor.Processor(), name, channel, sampleRate, queueData);
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
                    
                    TtlSensor sensor;
                    bool success = channelHandleToSensor.TryGetValue(channelHandle, out sensor);
                    Debug.Assert(success);

                    sensor.AddData(data);

                    samplesAvailable -= data.Length;
                }

                channelHandle = session.GetNextChannelHND();
            }

            return;
        }


        


        


        const int MAX_SAMPLES = 255;

        readonly Dictionary<int, TtlSensor> channelHandleToSensor;
        
        //Threading stuff
        readonly Thread sessionThread;
        Control sessionThreadControl;  //is used so we can Invoke() it and call stuff on the session thread  //!!(use ISynchronizeInvoke instead?)
        Exception sessionThreadInitException;
        readonly ApplicationContext applicationContext;  //Allows us to exit the message pump on the session thread (usually it just listens to the Closed() event on the main form)

        ITTLLive2 session;
        Timer dataTimer;
        Timer notificationTimer;
        

        int sensorsStarted;
    }
}
