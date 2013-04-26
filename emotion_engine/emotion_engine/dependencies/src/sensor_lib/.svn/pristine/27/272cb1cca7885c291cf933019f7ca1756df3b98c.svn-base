using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TTLLiveCtrlLib;
using SensorLib.Util;

namespace SensorLib.ThoughtTechnologies
{
    //Lazy singleton that creates ITTLive2 COM Object
    static class TtlLiveCom
    {

        #region Public Methods

        public static ITTLLive2 Get()
        {
            if (disposed)
                throw new Exception("Object is disposed!");

            if (variableCreated)
                return variable;

            variable = CreateVariable();
            variableCreated = true;

            return variable;
        }


        public static void Dispose()
        {
            if (disposed)
                throw new Exception("Object is disposed!");

            ReleaseVariable();
            disposed = true;
            return;
        }

        public static bool IsDisposed { get { return disposed; } }

        #endregion


        #region Generic Create/Release Methods

        static ITTLLive2 CreateVariable()
        {
            return CreateComInstance();
        }

        static void ReleaseVariable()
        {
            ReleaseComInstance(variable);
            return;
        }

        #endregion


        #region Specific Create/Release Methods

        public static ITTLLive2 CreateComInstance()
        {
            //Initialize COM Library for this thread
            uint options = (uint)(Ole32Methods.CoInit.COINIT_SPEED_OVER_MEMORY | Ole32Methods.CoInit.COINIT_APARTMENTTHREADED); //!!Haven't looked into this but looks useful
            Ole32Methods.HRESULT result = Ole32Methods.CoInitializeEx(null, options);
            if (result < 0)
            {
                if ((uint)result == 0x80010106)
                {
                    throw new Exception("Could not initialize COM environment: Thread mode (STA/MTA) is different.");
                }
                else
                {
                    throw new Exception("Could not initialize COM environment.");
                }
            }

            //Create instance of TTLLive object and get pointer to its ITTLLive instance
            Guid TTLLiveClassGuid = typeof(TTLLiveClass).GUID;
            //Guid TTLLiveClassGuid = typeof(TTLLive).GUID;
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

        public static void ReleaseComInstance(ITTLLive2 session)
        {
            //Release COM object
            //!!Do we have to release instance?

            //Deinitialize COM Library
            Ole32Methods.CoUninitialize();
            return;
        }

        #endregion


        #region Private Data

        static ITTLLive2 variable;
        static bool variableCreated = false;
        static bool disposed = false;

        #endregion

    }
}
