using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using TTLLiveCtrlLib;
using System.Runtime.InteropServices;
//using stdole;

using System.Diagnostics;

namespace SensorLib
{
    //Thought Technology Sensors

    //!!Only supports one encoder
    
    //NOTE: This class is not thread-safe!  
    //This class makes sure that we are always doing fast reads by reading with the same thread we created the COM object on
    //This class also makes sure that we never block the thread doing all the reading (the blocking seems to mess up the data stream (all zeros))
    //This class also ensures the uninitialization of the COM environment
    partial class LiveSessionTtlOld /*: ITtlSensors */
    {
        /*
        public LiveSessionTtlOld()
        {
            
            threadInitialized = new object();
            lock (threadInitialized)
            {
                threadExit = false;
                dataThread = new Thread(new ThreadStart(StartThread));
                dataThread.SetApartmentState(ApartmentState.MTA);
                dataThread.IsBackground = true;

                bool success;
                Monitor.Enter(threadInitialized);
                {
                    dataThread.Start();

                    //Wait for thread to do its thing
                    success = Monitor.Wait(threadInitialized, 5000);
                }
                Monitor.Exit(threadInitialized);

                //Check for initialization error or timeout
                if (initException != null)
                {
                    //Signal thread to exit
                    threadExit = true;
                    dataThread.Join();
                    throw initException;
                }
                else if (!success)
                {
                    //Kill the thread
                    threadExit = true;
                    dataThread.Abort();
                    throw new Exception("Thread took too long to initialize!");
                }
                
            }

            return;
        }


        public void Finish()
        {
            Monitor.Enter(ttlEncoderThreadLock);
            {
                //Signal thread to exit
                threadExit = true;
                Monitor.Pulse(ttlEncoderThreadLock);
            }
            Monitor.Exit(ttlEncoderThreadLock);

            bool success = dataThread.Join(5000);
            if (!success)
                dataThread.Abort();

            return;
        }

        
        public TtlEncoder[] GetEncoders()
        {
            TTLEncoder[] newEncoders;

            //Get encoder
            lock (sessionLock)
            {
                //Check if there are any encoders
                newEncoders = GetEncoders(session);
            }


            //Drop old encoders
            foreach (TtlEncoder encoder in encoders)
            {
                if (newEncoders.Contains(encoder.encoder))
                    continue;


            }
            //Create new encoders
            foreach (TTLEncoder encoder in newEncoders)
            {
                if (encoders.Contains(encoder))
                    continue;

                
                {
                    //Remove the current encoder
                    //!!

                    //ttlEncoder.EncoderStarted -= new EventHandler(ttlEncoder_EncoderStarted);
                    //ttlEncoder.EncoderStopped -= new EventHandler(ttlEncoder_EncoderStopped);
                    //!!Dispose, drop?
                    //ttlEncoder = null;

                    encoder = null;
                    validEncoder = false;
                }
            }
            


            //Set up encoders for the list
            TtlEncoder[] ttlEncoders = new TtlEncoder[encoders.Length];
            for(int i=0; i<encoders.Length; i++)
            {
                //Create our sensorlib encoder
                ttlEncoders[i] = new TtlEncoder(encoders[i], AddChannel, RemoveChannel);
                ttlEncoders[i].EncoderStarted += new EventHandler(ttlEncoder_EncoderStarted);
                ttlEncoders[i].EncoderStopped += new EventHandler(ttlEncoder_EncoderStopped);
            }
            
            /*
            //Signal that we now have a valid encoder
            Monitor.Enter(ttlEncoderThreadLock);
            {
                validEncoder = true;
                Monitor.Pulse(ttlEncoderThreadLock);
            }
            Monitor.Exit(ttlEncoderThreadLock);
            *//*

            return ttlEncoders;
        }

        public ITtlSensor CreateSensor(SensorType type, Channel channel, String name, bool queueData)
        {
            //if (channelList.ContainsKey(channel))
            //    throw new Exception("Already a sensor registered on that channel!");

            //!!Make sure we don't have a duplicate name!
            //if (channelList.ContainsValue(ttlChannel))
            //    throw new Exception("Already have the sensor registered!");

            /*
            TTLChannel tTLChannel = addChannelDelegate(channel);
            float sampleRate = tTLChannel.NominalSampleRate;

            ttlChannel = CreateSensor(type, name, channel, tTLChannel, sampleRate, queueData);

            ttlChannel.ChannelStarted += new EventHandler(channel_ChannelStarted);
            ttlChannel.ChannelStopped += new EventHandler(channel_ChannelStopped);

            channelList.Add(ttlChannel.Channel, ttlChannel);
            */

            //ITtlSensor sensor = CreateSensor(type, channel, name

            //return sensor;
        /*
            throw new NotImplementedException();
        }

        public void RemoveSensor(ITtlSensor sensor)
        {
            throw new NotImplementedException();
        }


        public void StartChannel(int channelHnd)
        {

            return;
        }

        public void StopChannel(int channelHnd)
        {
            return;
        }

        //Dictionary<ITtlSensor, TTLEncoder> sensorToEncoder;
        //Dictionary<TTLEncoder, 


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
        *//*



        
        void StartThread()
        {
            //Initialize COM object
            Monitor.Enter(threadInitialized);
            {
                try
                {
                    session = TtlLiveSingleton.Get();
                }
                catch (Exception ex)
                {
                    initException = ex;

                    //!!Close connections?
                    Monitor.Pulse(threadInitialized);
                    Monitor.Exit(threadInitialized);
                    TtlLiveSingleton.Dispose();
                    return;
                }

                Monitor.Pulse(threadInitialized);

            }
            Monitor.Exit(threadInitialized);
            

            //Use thread to gather data from COM object
            DataLoop();

            lock (sessionLock)
            {
                session.CloseConnections();
                TtlLiveSingleton.Dispose();
            }
            return;
        }


        void DataLoop()
        {

            while (!threadExit)
            {

                //Get data from all used encoders
                foreach (TtlEncoder encoder in encoderToIsRunning)
                {
                    
                    Dictionary<int, List<ITtlSensor>>.Enumerator channelEnumerator = channelHndToSensor.GetEnumerator();
                    if(encoderIsRunning)
                    {
                        KeyValuePair<int, List<ITtlSensor>> pair = channelEnumerator.Current;

                        //Get values
                        int channelHnd = pair.Key;
                        List<ITtlSensor> sensors = pair.Value;

                        //Get data from encoder
                        float[] data = (float[])session.ReadChannelDataVT(channelHnd, 255);

                        //Give data to all sensors on this channel
                        foreach (ITtlSensor sensor in sensors)
                        {
                            throw new NotImplementedException(); //!!
                        }
                        session.set_ChannelActive(
                        //Update enumerator
                        bool isNotAtEnd = channelEnumerator.MoveNext();
                        if (!isNotAtEnd)
                            channelEnumerator = channelHndToSensor.GetEnumerator();
                    }
                }

                Monitor.Enter(ttlEncoderThreadLock);
                {
                    if ((!encoderIsRunning || !validEncoder) && !threadExit)
                    {
                        Monitor.Wait(ttlEncoderThreadLock);
                    }
                }
                Monitor.Exit(ttlEncoderThreadLock);

            }

            return;
        }

        

        void ttlEncoder_EncoderStarted(object sender, EventArgs e)
        {
            lock (sessionLock)
            {
                session.StartChannels();
                Monitor.Enter(ttlEncoderThreadLock);
                {
                    encoderIsRunning = true;
                    Monitor.Pulse(ttlEncoderThreadLock);
                }
                Monitor.Exit(ttlEncoderThreadLock);
            }
            return;
        }

        void ttlEncoder_EncoderStopped(object sender, EventArgs e)
        {
            lock (sessionLock)
            {
                session.StopChannels();
                encoderIsRunning = false;
            }
            return;
        }



        //!!Make sure there are no open channels in the encoder! (drop all channels)
        TTLEncoder[] GetEncoders(ITTLLive2 session)
        {
            //Open connections to find all encoders connected to computer
            int scanned = 0;
            int detected = 0;
            session.OpenConnections(
                        (int)TTLLiveCtrlLib.TTLAPI_OPENCONNECTIONS_CMD_BITS.TTLAPI_OCCMD_AUTODETECT
                    , 1000
                    , ref scanned
                    , ref detected
                    );
            if (session.EncoderCount <= 0)
            {
                return new TTLEncoder[0];
            }

            //Get encoders
            int encoderCount = session.EncoderCount;
            TTLEncoder[] encoders = new TTLEncoder[encoderCount];
            for(int i=0; i<encoderCount; i++)
            {
                encoders[i] = session.Encoder[i];
            }

            return encoders;
        }


        TTLChannel AddChannel(TTLEncoder encoder, Channel channel)
        {

            /*
            //Set up all channels automatically
            session.AutoSetupChannels();
            if (session.ChannelCount <= 0)
            {
                throw new Exception("Could not find any channels.");
            }
            *//*

            //Get the first available channel on the encoder
            int channelHandle = -1;  //passing in -1 means AddChannel() will just return the first available channel
            encoder.AddChannel((TTLAPI_CHANNELS)channel, ref channelHandle);
            
            //Get the channel instance that we just added
            TTLChannel channel = session.get_Channel(channelHandle);

            return channel;
        }


        void RemoveChannel(TTLChannel channel)
        {
            lock (sessionLock)
            {
                session.DropChannel(channel.HND);
            }
            return;
        }

        
        //Threading stuff
        object threadInitialized;
        Thread dataThread;
        bool threadExit;
        
        ITTLLive2 session;
        Exception initException = null; //is used by the other thread on initialization so that the user can catch it

        TTLEncoder[] encoders;
        bool encoderIsRunning = false;
        bool validEncoder = false;

        //Locks
        object ttlEncoderThreadLock = new object();   //Is used for data thread control
        object ttlEncoderLock = new object(); //Is used to lock the encoder
        object sessionLock = new object();

        
        readonly List<TtlEncoder> encoders = new List<TtlEncoder>();

        struct TtlEncoder
        {
            public TTLEncoder encoder;
            public int channelsStarted;
            public bool isRunning { get { return channelsStarted > 0; } }

            public List<TtlChannel> sensors;
        }

        struct TtlChannel
        {
            public int channelHnd;
            public TSQueue<float> dataQ;
        }
               */
    }
}
