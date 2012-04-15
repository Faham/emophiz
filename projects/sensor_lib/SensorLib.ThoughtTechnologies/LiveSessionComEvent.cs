using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using TTLLiveCtrlLib;
using System.Runtime.InteropServices;
//using stdole;


namespace SensorLib
{

    //!!Only supports one encoder
    
    //NOTE: This class is not thread-safe!  
    //This class makes sure that we are always doing fast reads by reading with the same thread we created the COM object on
    //This class also makes sure that we never block the thread doing all the reading (the blocking seems to mess up the data stream (all zeros))
    //This class also ensures the uninitialization of the COM environment
    class LiveSessionComEvent : LiveSession, ILiveSession
    {

        public LiveSessionComEvent()
        {
            connectException = false;
            threadInitialized = new object();
            lock (threadInitialized)
            {
                threadExit = false;
                dataThread = new Thread(new ThreadStart(StartThread));
                dataThread.IsBackground = true;
                dataThread.Start();

                //Wait for thread to do its thing
                Monitor.Wait(threadInitialized, 1000);  //!! Return code!

                if (connectException)
                {
                    //Signal thread to exit
                    threadExit = true;
                    dataThread.Join();
                    throw new Exception("Could not connect!");
                }
            }

            dictionary = new Dictionary<Channel, ChannelQueue>();

            return;
        }


        public void Finish()
        {
            //Signal thread to exit
            threadExit = true;
            dataThread.Join();
            return;
        }

        

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

        float BatteryPercentage
        {
            get { return encoder.BatteryLevelPct; }
        }

        float BatteryTimeLeft
        {
            get { return encoder.BatteryTimeLeft; }
        }
        
        /////////////////////
        // Factory Methods //
        /////////////////////

        public ITempSensor CreateTempSensor(Channel channel)
        {
            return CreateTempSensor(channel, false);
        }

        public ITempSensor CreateTempSensor(Channel channel, bool dataAvailable)
        {
            return new TempSensor(AddChannel(channel), dataAvailable);
        }

        public IBvpSensor CreateBvpSensor(Channel channel)
        {
            return CreateBvpSensor(channel, false);
        }

        public IBvpSensor CreateBvpSensor(Channel channel, bool dataAvailable)
        {
            return new BvpSensor(AddChannel(channel), dataAvailable);
        }

        public IStrainSensor CreateStrainSensor(Channel channel)
        {
            return CreateStrainSensor(channel, false);
        }

        public IStrainSensor CreateStrainSensor(Channel channel, bool dataAvailable)
        {
            return new StrainSensor(AddChannel(channel), dataAvailable);
        }

        public IBrainSensor CreateBrainSensor(Channel channel)
        {
            return CreateBrainSensor(channel, false);
        }

        public IBrainSensor CreateBrainSensor(Channel channel, bool dataAvailable)
        {
            return new BrainSensor(AddChannel(channel), dataAvailable);
        }
        
        public IRawSensor CreateRawSensor(Channel channel)
        {
            return new RawSensor(AddChannel(channel));
        }


        TSQueue<float> AddChannel(Channel channel)
        {
            
            lock(dictionary)
            {
                //Check if channel is already in use
                if(dictionary.ContainsKey(channel))
                    throw new Exception(String.Format("Session already uses {0}", channel.ToString()[0])); //!!Possible bug

                TTLChannel ttlChannel = GetChannel(encoder, channel);
                TSQueue<float> queue = new TSQueue<float>();
                dictionary.Add(channel, new ChannelQueue(ttlChannel, queue));
                return queue;
            }
        }

        /*
        //!!This method should only be used if someone wants to make their own monitor!
        //!!   You should not read data from a channel that is already having data read from!
        public float[] GetData(Channel channel, int maxSamples)  //!!This should not be public!
        {
            return queues[(int)channel].Dequeue(maxSamples);  //!!
        }
        */


        static ITTLLive2 CreateComInstance()
        {
            //Initialize COM Library for this thread
            uint options = (uint)(Ole32Methods.CoInit.COINIT_SPEED_OVER_MEMORY); //!!Haven't looked into this but looks useful
            Ole32Methods.HRESULT result = Ole32Methods.CoInitializeEx(null, options);
            if (result < 0)
                throw new Exception("Could not initialize COM environment.");

            //Create instance of TTLLive object and get pointer to its ITTLLive instance
            Guid TTLLiveClassGuid = typeof(TTLLiveClass).GUID;
            Guid ITTLLiveGuid = typeof(ITTLLive).GUID;

            //Create an instance of the COM object TTLLiveClass
            //NOTE: only use CoCreateInstance to make ONE instance on a LOCAL computer; possibly use CoGetClassObject in order to efficiently make multiple instances
            object comObject;
            comObject = Ole32Methods.CoCreateInstance(ref TTLLiveClassGuid, null, (int)Ole32Methods.CLSCTX.CLSCTX_INPROC_SERVER, ref ITTLLiveGuid);
            if (comObject == null)
                throw new Exception("Could not create instance.");


            //Return interface of object
            return (ITTLLive2)comObject;
        }

        static void ReleaseComInstance(ITTLLive2 session)
        {
            //Release COM object
            //!!Do we have to release instance?

            //Deinitialize COM Library
            Ole32Methods.CoUninitialize();
            return;
        }

        void StartThread()
        {
            //Initialize COM object
            lock (threadInitialized)
            {
                try
                {
                    session = CreateComInstance();
                    encoder = GetEncoder(session);
                }
                catch (Exception)
                {
                    connectException = true;
                }
                Monitor.Pulse(threadInitialized);

                if (connectException)  //!!Close connections?
                {
                    ReleaseComInstance(session);
                    return;
                }
            }

            //Use thread to gather data from COM object
            DataLoop();

            //!!End channels and queues
            /*
            for (int i = 0; i < channels.Length; i++)
            {

                queues[i].SignalExit();
                queues.

            }*/

            session.CloseConnections();
            ReleaseComInstance(session);
            return;
        }

        void DataLoop()
        {
            const int MAX_SAMPLES = 256;

            int channelIndex = 0;
            while (!threadExit)
            {
                if (!channelsStarted)
                    continue;
                
                //TTLChannel liveChannel = session.GetFirstChannel();  //!!Just doing first channel right now

                TTLChannel channel;
                TSQueue<float> queue;

                lock (dictionary)
                {
                    channelIndex++;
                    ChannelQueue[] channelQueue = dictionary.Values.ToArray();
                    channelIndex %= channelQueue.Length;

                    channel = channelQueue[channelIndex].channel;
                    queue = channelQueue[channelIndex].queue;
                }

                //Check if data is available
                int available = channel.SamplesAvailable;
                if (available > MAX_SAMPLES) available = MAX_SAMPLES;


                if (available > 0)
                {
                    //float[] data = new float[available];
                    //liveChannel.ReadData(out data[0], ref available);
                    float[] data = (float[])channel.ReadDataVT(available);

                    if (queue.Enqueue(data) == false)
                        Console.WriteLine("Error: Queue Overflow!");

                }
                //else
                //{
                //   Console.WriteLine("No Data.");
                //}
                
            }

            return;
        }


        //!!Make sure there are no open channels in the encoder! (drop all channels)
        TTLEncoder GetEncoder(ITTLLive2 session)
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
                throw new Exception("Could not find any encoders.");
            }

            //Take first encoder
            return session.GetFirstEncoder();
        }

        
        TTLChannel GetChannel(TTLEncoder encoder, Channel channel)
        {
            /*
            //Set up all channels automatically
            session.AutoSetupChannels();
            if (session.ChannelCount <= 0)
            {
                throw new Exception("Could not find any channels.");
            }
            */

            //Get the first channel on the first encoder
            int channel1Handle = -1;  //passing in -1 means AddChannel() will just return the first available channel
            //encoder.AddChannel(TTLAPI_CHANNELS.TTLAPI_CHANNEL_A, ref channel1Handle);
            encoder.AddChannel((TTLAPI_CHANNELS)channel, ref channel1Handle);
            TTLChannel ch1 = session.get_Channel(channel1Handle);

            return ch1;
        }
        


        object threadInitialized;
        Thread dataThread;
        bool threadExit;

        volatile bool channelsStarted = false;
        
        ITTLLive2 session;
        TTLEncoder encoder;

        struct ChannelQueue
        {
            public ChannelQueue(TTLChannel channel_, TSQueue<float> queue_)
            {
                channel = channel_;
                queue = queue_;
                return;
            }
            public readonly TTLChannel channel;
            public readonly TSQueue<float> queue;
        }

        Dictionary<Channel, ChannelQueue> dictionary;
        bool connectException;  
    }
}
