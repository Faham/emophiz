using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Threading;


namespace SensorLib.Util
{
    /// <summary>
    /// Creates a thread with a message pump so you can use events on it.
    /// </summary>
    public class ApplicationThread
    {
        
        public ApplicationThread(Action threadInit, Action threadDeInit, int millisecondsTimeout)
        {
            
            this.threadInit = threadInit;
            this.threadDeInit = threadDeInit;

            applicationContext = new ApplicationContext();
            //threadControl = new Control();
            //threadControl.CreateControl();

            thread = new SuperThread(ThreadStart, millisecondsTimeout);
            return;
        }


        public void Start()
        {
            thread.Start();
            return;
        }

        
        public void Join()
        {
            //Exit message pump
            applicationContext.ExitThread();

            //Exit anyone invoking the thread control
            //threadControl.Dispose();

            //Wait for the thread to finish
            thread.Join();

            return;
        }


        void ThreadStart()
        {
            
            // Start with the thread initializer
            threadInit();

            // Tell main thread that we are initialized
            thread.InitDone();

            // Create a message pump for this thread
            Application.Run(applicationContext);

            // Start the thread deinitializer
            threadDeInit();

            return;
        }





        readonly Action threadInit;
        readonly Action threadDeInit;
        readonly SuperThread thread;
        readonly ApplicationContext applicationContext;
        //readonly Control threadControl;


    }
}
