using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib
{
    //Wrapper to make LiveSessionTtl a singleton
    partial class LiveSessionTtl : ILiveSessionTtl
    {
        public static ILiveSessionTtl CreateInstance()
        {
            return CreateInstance(false);
        }

        public static ILiveSessionTtl CreateInstance(bool simulate_)
        {
            //NOTE: The COM class is configured for apartment threading which means it is slow (because of marshalling) to use instance if not the creating thread

            lock (singletonMutex)
            {
                if (referenceCount == 0)
                {
                    //!!if (simulate_)
                    //    liveSession = new LiveSessionTtlSim();
                    //else
                        liveSession = new LiveSessionTtl();
                }
                referenceCount++;
                Simulate = simulate_;
            }


            return liveSession;
        }

        public static void ReleaseInstance()
        {
            lock (singletonMutex)
            {
                if (referenceCount == 0)
                    throw new Exception("No instances to remove!");

                referenceCount--;
                if (referenceCount == 0)
                {
                    liveSession.Dispose();
                    liveSession = null;
                    simulateSet = false;
                }
            }

            return;
        }

        static object singletonMutex = new object();
        static ILiveSessionTtl liveSession = null;
        static int referenceCount = 0;


        static bool Simulate
        {
            set
            {
                //!!if (simulateSet)
                //!!    throw new Exception("You cannot use a simulation and non-simulation session in the same application!");

                simulate = value;
                simulateSet = true;
            }

            get
            {
                return simulate;
            }
        }

        static bool simulate = false;  //NOTE: You should NOT set this variable directly!  Only set it through Simulate
        static bool simulateSet = false; //makes sure you cannot change simulate once it is set
    }
}
