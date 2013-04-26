using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;


namespace SensorLib.Util
{
    /// <summary>
    /// Creates a thread and blocks the creating thread until the created thread has initialized.
    /// </summary>
    public class SuperThread
    {
        
        public SuperThread(ThreadStart threadStart, int millisecondsTimeout)
        {
            
            this.threadStart = threadStart;
            this.millisecondsTimeout = millisecondsTimeout;

            initException = null;
            
            thread = new Thread(ThreadStart);
            thread.IsBackground = true;
            return;
        }


        public void Start()
        {
            bool noTimeout;
            Monitor.Enter(initThreadLock);
            {
                //Create thread and start it
                thread.Start();

                //Wait for thread to initialize
                noTimeout = Monitor.Wait(initThreadLock, millisecondsTimeout);
            }
            Monitor.Exit(initThreadLock);

            //Check for initialization error
            if (initException != null)
            {
                throw initException;
            }
            else if (!noTimeout)
            {
                throw new TimeoutException();
            }

            return;
        }


        public void InitDone()
        {
            //Tell main thread that we are done initializing
            Monitor.Enter(initThreadLock);
            {
                Monitor.Pulse(initThreadLock);
            }
            Monitor.Exit(initThreadLock);
            return;
        }

        
        public void Join()
        {
            thread.Join();
            return;
        }


        void ThreadStart()
        {
            try
            {
                threadStart();
            }
            catch (Exception ex)
            {
                //Set the exception for the main thread
                initException = ex;
            }

            //Tell main thread that we are done initializing
            InitDone();

            return;
        }





        readonly ThreadStart threadStart;
        readonly int millisecondsTimeout;
        readonly Thread thread;
        readonly object initThreadLock = new object();

        Exception initException;

    }
}
