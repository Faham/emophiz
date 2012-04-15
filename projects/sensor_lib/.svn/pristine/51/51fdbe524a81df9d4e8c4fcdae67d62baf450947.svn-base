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
    partial class LiveSessionTtlReallyOld //: ITtlSensors
    {
        /*
        public LiveSessionTtlReallyOld()
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

                if (initException != null || !success)
                {
                    //Signal thread to exit
                    threadExit = true;
                    dataThread.Join();
                    throw initException;
                }
            }

            return;
        }


        public void Finish()
        {
            Monitor.Enter(ttlEncoderLock);
            {
                //Signal thread to exit
                threadExit = true;
                Monitor.Pulse(ttlEncoderLock);
            }
            Monitor.Exit(ttlEncoderLock);

            dataThread.Join();
            return;
        }

        public TtlEncoder[] GetEncoders()
        {
            //Check if there are any encoders
            TTLEncoder firstEncoder = GetEncoder(session);
            if (firstEncoder == null)
            {
                //Check if we had an encoder
                if (ttlEncoder != null)
                {
                    ttlEncoder.EncoderStarted -= new EventHandler(ttlEncoder_EncoderStarted);
                    ttlEncoder.EncoderStopped -= new EventHandler(ttlEncoder_EncoderStopped);
                    //!!Dispose, drop?
                    ttlEncoder = null;
                }

                validEncoder = false;
                return new TtlEncoder[0];
            }

            //Check if we already have an encoder
            if (ttlEncoder != null)
                return new TtlEncoder[] { ttlEncoder };
            
            //Create our sensorlib encoder
            ttlEncoder = new TtlEncoder(firstEncoder, AddChannel, RemoveChannel);
            ttlEncoder.EncoderStarted += new EventHandler(ttlEncoder_EncoderStarted);
            ttlEncoder.EncoderStopped += new EventHandler(ttlEncoder_EncoderStopped);

            //Signal that we now have a valid encoder
            Monitor.Enter(ttlEncoderLock);
            {
                validEncoder = true;
                Monitor.Pulse(ttlEncoderLock);
            }
            Monitor.Exit(ttlEncoderLock);

            return new TtlEncoder[] { ttlEncoder };
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
        *//*



        
        void StartThread()
        {
            lock (sessionLock)
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
            }

            //Use thread to gather data from COM object
            DataLoop();

            lock (sessionLock)
            {
                session.CloseConnections();
                TtlLiveSingleton.Dispose();
            }
            return;
        }


        void ttlEncoder_EncoderStarted(object sender, EventArgs e)
        {
            lock (sessionLock)
            {
                session.StartChannels();
                Monitor.Enter(ttlEncoderLock);
                {
                    encoderIsRunning = true;
                    Monitor.Pulse(ttlEncoderLock);
                }
                Monitor.Exit(ttlEncoderLock);
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


        void DataLoop()
        {

            while (!threadExit)
            {
                
                if (ttlEncoder != null && ttlEncoder.IsRunning)  //!!channelsStarted should be false if we are not using flexComp
                {
                    ttlEncoder.ReadData();
                }
                else
                {
                    Monitor.Enter(ttlEncoderLock);
                    {
                        if ((!encoderIsRunning || !validEncoder) && !threadExit)
                        {
                            Monitor.Wait(ttlEncoderLock);
                        }
                    }
                    Monitor.Exit(ttlEncoderLock);
                }
                

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
                return null;
            }

            //Take first encoder
            return session.GetFirstEncoder();
            
        }


        TTLChannel AddChannel(Channel channel)
        {

            /*
            //Set up all channels automatically
            session.AutoSetupChannels();
            if (session.ChannelCount <= 0)
            {
                throw new Exception("Could not find any channels.");
            }
            *//*

            TTLChannel ch1;

            lock (sessionLock)
            {

                //Get the first available channel on the encoder
                int channel1Handle = -1;  //passing in -1 means AddChannel() will just return the first available channel
                GetEncoders()[0].Encoder.AddChannel((TTLAPI_CHANNELS)channel, ref channel1Handle);

                //Get the channel instance that we just added
                ch1 = session.get_Channel(channel1Handle);
            }
            return ch1;
        }


        void RemoveChannel(TTLChannel channel)
        {
            lock (sessionLock)
            {
                session.DropChannel(channel.HND);
            }
            return;
        }

        

        object threadInitialized;
        Thread dataThread;
        bool threadExit;
        
        ITTLLive2 session;

        Exception initException = null; //is used by the other thread on initialization so that the user can catch it

        TtlEncoder ttlEncoder;
        bool encoderIsRunning = false;
        bool validEncoder = false;
        object ttlEncoderLock = new object();
        object sessionLock = new object();
               */
    }
}
