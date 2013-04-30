using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using Tobii.Eyetracking.Sdk;
using Tobii.Eyetracking.Sdk.Time;
using Tobii.Eyetracking.Sdk.Exceptions;

using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;

using SensorLib.Tobii.Calibration;

namespace SensorLib.Tobii
{

    using TobiiEyeTrackerInfo = global::Tobii.Eyetracking.Sdk.EyetrackerInfo;
    using Point2D = SensorLib.Util.Point2D;
    using Point3D = SensorLib.Util.Point3D;
    using Point2DTobii = global::Tobii.Eyetracking.Sdk.Point2D;
    using Point3DTobii = global::Tobii.Eyetracking.Sdk.Point3D;


    [Serializable]
    public class EyeTracker : Device<IEyeTrackerInfo, Sample<EyeData>>, IEyeTracker
    {


        #region Getting Devices

        //Note: Is only called when first instance of EyeTracker is created
        static EyeTracker()
        {
            //Initialize Tobii SDK eyetracking library
            Library.Init();

            userThreadControl = new Control();
            userThreadControl.CreateControl();

            eyeTrackers = new Dictionary<String, TobiiEyeTrackerInfo>();

            clock = new Clock();

            //Set up eye tracker browser
            EyetrackerBrowser browser = new EyetrackerBrowser(EventThreadingOptions.BackgroundThread /*EventThreadingOptions.CallingThread*/);
            browser.EyetrackerFound += browser_EyetrackerFound;
            browser.EyetrackerUpdated += browser_EyetrackerUpdated;
            browser.EyetrackerRemoved += browser_EyetrackerRemoved;

            browser.Start();
            initTime = DateTime.Now;
            
            return;
        }
        

        public static IEyeTrackerInfo[] GetEyeTrackers()
        {
            //Allow us to find any eye trackers that were connected when we initialized
            int millisecondsSinceInit = (int)(DateTime.Now - initTime).TotalMilliseconds;
            if (millisecondsSinceInit < minMillisecondsSinceInit)
                Thread.Sleep(minMillisecondsSinceInit - millisecondsSinceInit);


            lock (eyeTrackers)
            {
                return new List<TobiiEyeTrackerInfo>(eyeTrackers.Values).ConvertAll(x => new EyeTrackerInfo(x)).ToArray();
            }
        }


        static void browser_EyetrackerFound(object sender, EyetrackerInfoEventArgs e)
        {
            
            String productId = e.EyetrackerInfo.ProductId;

            lock (eyeTrackers)
            {
                eyeTrackers.Add(productId, e.EyetrackerInfo);
            }

            if (EyeTrackerAdded != null)
                userThreadControl.Invoke(EyeTrackerAdded, new EyeTrackerInfo(e.EyetrackerInfo));

            return;
        }

        static void browser_EyetrackerUpdated(object sender, EyetrackerInfoEventArgs e)
        {
            String productId = e.EyetrackerInfo.ProductId;

            lock (eyeTrackers)
            {
                eyeTrackers[productId] = e.EyetrackerInfo;
            }

            //!!Send event?
            return;
        }

        static void browser_EyetrackerRemoved(object sender, EyetrackerInfoEventArgs e)
        {
            String productId = e.EyetrackerInfo.ProductId;

            lock (eyeTrackers)
            {
                bool success = eyeTrackers.Remove(productId);
                if(!success)
                    throw new Exception("Remove: Tried to remove non-existing eye tracker");
            }
            
            if (EyeTrackerRemoved != null)
                userThreadControl.Invoke(EyeTrackerRemoved, e.EyetrackerInfo);

            return;
        }


        public static event Action<IEyeTrackerInfo> EyeTrackerAdded;
        public static event Action<IEyeTrackerInfo> EyeTrackerRemoved;

        
        const int minMillisecondsSinceInit = 1000;
        
        readonly static DateTime initTime;
        readonly static Control userThreadControl;
        readonly static Dictionary<String, TobiiEyeTrackerInfo> eyeTrackers;
        readonly static Clock clock;


        #endregion


        #region Connecting To Device

        public static IEyeTracker Connect(IEyeTrackerInfo info)
        {
            return new EyeTracker(info);
        }

        #endregion
        


        //////////////////////
        // Instance Methods //
        //////////////////////


        // Constructors //

        protected EyeTracker(IEyeTrackerInfo info)
	    {
            this.info = info;

            TobiiEyeTrackerInfo tobiiEyeTrackerInfo;
            try
            {
                tobiiEyeTrackerInfo = eyeTrackers[info.ProductId];
            }
            catch
            {
                throw new ArgumentException("Could not find device.");
            }

            try
            {
                tobiiEyeTracker = EyetrackerFactory.CreateEyetracker(tobiiEyeTrackerInfo, EventThreadingOptions.BackgroundThread);
                tobiiEyeTracker.ConnectionError += new EventHandler<ConnectionErrorEventArgs>(eyeTracker_ConnectionError);

                syncManager = new SyncManager(clock, tobiiEyeTrackerInfo);

                tobiiEyeTracker.GazeDataReceived += new EventHandler<GazeDataEventArgs>(eyeTracker_GazeDataReceived);
                tobiiEyeTracker.FramerateChanged += new EventHandler<FramerateChangedEventArgs>(tobiiEyeTracker_FramerateChanged);

                //!!tobiiEyeTracker.RealTimeGazeData = true;
                calibration = tobiiEyeTracker.GetCalibration();
                findingTimeOffset = true;
                startTime = DateTime.Now;
            }
            catch (EyetrackerException ee)
            {

                Dispose();

                if (ee.ErrorCode == 0x20000402)
                {
                    throw new Exception("Failed to upgrade protocol. " +
                        "This probably means that the firmware needs" +
                        " to be upgraded to a version that supports the new sdk.");
                }
                else
                {
                    throw new Exception("Eyetracker responded with error " + ee);
                }

            }
            catch (Exception)
            {
                Dispose();
                throw new Exception("Connection Failed: Could not connect to eyetracker.");                
            }

            //var availableFramerates = _connectedTracker.EnumerateFramerates();
            //int fpsIndex = availableFramerates.IndexOf(framerate);
            //eyeTracker.SetFramerate(fpsDialog.CurrentFramerate);

            

            return;
	    }



        // Public Properties //

        public override IEyeTrackerInfo Info { get { return info; } }

        public DateTime StartTime { get { return startTime; } }

        
        // Public Methods //


        public void Calibrate()
        {
            
            EyeCalibrator calibrator = new EyeCalibrator();
            global::Tobii.Eyetracking.Sdk.Calibration newCalibration = calibrator.RunCalibration(tobiiEyeTracker);
            if (newCalibration == null)
                throw new Exception("Not enough data to create a calibration (or calibration aborted).");

            calibration = newCalibration;
            return;
        }

        public Image ViewCalibration()
        {
            Debug.Assert(calibration != null);

            return CalibrationViewer.GetCalibrationImage(calibration);
        }

        public void LoadCalibration(String path)
        {
            using (Stream stream = File.OpenRead(path))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                byte[] data = reader.ReadBytes((int)stream.Length);
                global::Tobii.Eyetracking.Sdk.Calibration calibration = new global::Tobii.Eyetracking.Sdk.Calibration(data);
                tobiiEyeTracker.SetCalibration(calibration);
            }
        }

        public void SaveCalibration(String path)
        {

            global::Tobii.Eyetracking.Sdk.Calibration calibration = tobiiEyeTracker.GetCalibration();

            using (Stream stream = File.OpenWrite(path))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(calibration.RawData);
            }

            return;
        }


        public IEyeSensor CreateSensor(String name, bool queueData)
        {
            EyeSensor sensor = new EyeSensor(this, tobiiEyeTracker.GetFramerate(), name, queueData);
            base.CreateSensor(sensor);
            return sensor;
        }



        /*
        public override string ToString()
        {
            if (_servicename != null)
                return _servicename;

            string presentation;
            if (_entry.name != "" && _entry.name != _entry.model)
                presentation = _entry.name + " ";
            else
                presentation = _entry.pid + " ";
            presentation += "(" + _entry.model + ")";
            return presentation;
        }
        */

        /*
        public bool IsRunning
        {
            get {
                if (_servicename == null)
                    return _entry.status == TetServiceEntryStatus.TetServiceEntryStatus_RUNNING;
                return false;
            }
        }
        */




        // Protected Methods //



        protected override void DeviceDispose()
        {
            if (tobiiEyeTracker != null)
            {
                tobiiEyeTracker.GazeDataReceived -= eyeTracker_GazeDataReceived;
                tobiiEyeTracker.Dispose();
                syncManager.Dispose();
            }

            return;
        }


        protected override void DeviceAddSensor(Sensor<Sample<EyeData>> sensor) { return; }
        protected override void DeviceRemoveSensor(Sensor<Sample<EyeData>> sensor) { return; }

        protected override void DeviceFirstSensorAdded()
        {
            tobiiEyeTracker.StartTracking();
            return;
        }

        protected override void DeviceLastSensorRemoved()
        {
            tobiiEyeTracker.StopTracking();
            return;
        }


        protected override void DeviceReceivedData(Sensors.Sensor<Sample<EyeData>> sensor, Sample<EyeData>[] data)
        {
            ((QueueInput<Sample<EyeData>>)sensor.Input).AddData(data);
            return;
        }



        // Private Methods //

        



        void eyeTracker_ConnectionError(object sender, ConnectionErrorEventArgs e)
        {
            // If the connection goes down we dispose 
            // the IAsyncEyetracker instance. This will release 
            // all resources held by the connection
            Drop();
            
            return;
        }

        
        void tobiiEyeTracker_FramerateChanged(object sender, FramerateChangedEventArgs e)
        {
            throw new NotSupportedException();
        }


        
        void eyeTracker_GazeDataReceived(object sender, GazeDataEventArgs e)
        {

            GazeDataItem gazeData = e.GazeDataItem;
            if (findingTimeOffset)
            {
                timeOffset = TimeSpan.FromMilliseconds(gazeData.TimeStamp / 1000) + (DateTime.Now - startTime);
                findingTimeOffset = false;
            }
            

            Sample<EyeData> eyeData = ConvertToEyeDataSample(gazeData);


            //We can just ignore the synchronization because we will just use the eye tracker hardware clock
            /*
            if (syncManager.SyncState.StateFlag == SyncStateFlag.Synchronized)
            {
                Int64 convertedTime = syncManager.RemoteToLocal(gazeData.TimeStamp);
                Int64 localTime = clock.GetTime();
            }
            else
            {
                Console.WriteLine("Warning: Sync state is " + syncManager.SyncState.StateFlag);
            }
            */

            ReceivedData(new Sample<EyeData>[] { eyeData });
            return;
        }

        

        Sample<EyeData> ConvertToEyeDataSample(GazeDataItem gazeData)
        {

            TimeSpan time = TimeSpan.FromMilliseconds(gazeData.TimeStamp / 1000) - timeOffset;

            EyeData.Eye left;
            {
                bool valid = gazeData.LeftValidity <= minValidCode;
                int validityCode = gazeData.LeftValidity;
                float diameter = gazeData.LeftPupilDiameter;

                Point3DTobii eyePosTobii = gazeData.LeftEyePosition3DRelative;
                Point3D eyePos = new Point3D((float)eyePosTobii.X, (float)eyePosTobii.Y, (float)eyePosTobii.Z);

                Point2DTobii gazePosTobii = gazeData.LeftGazePoint2D; //Can also do 3D
                Point2D gazePos = new Point2D((float)gazePosTobii.X, (float)gazePosTobii.Y);

                left = new EyeData.Eye(valid, validityCode, diameter, eyePos, gazePos);
            }

            EyeData.Eye right;
            {
                bool valid = gazeData.RightValidity <= minValidCode;
                int validityCode = gazeData.RightValidity;
                float diameter = gazeData.RightPupilDiameter;

                Point3DTobii eyePosTobii = gazeData.RightEyePosition3DRelative;
                Point3D eyePos = new Point3D((float)eyePosTobii.X, (float)eyePosTobii.Y, (float)eyePosTobii.Z);

                Point2DTobii gazePosTobii = gazeData.RightGazePoint2D; //Can also do 3D
                Point2D gazePos = new Point2D((float)gazePosTobii.X, (float)gazePosTobii.Y);

                right = new EyeData.Eye(valid, validityCode, diameter, eyePos, gazePos);
            }

            return new Sample<EyeData>(time, new EyeData(left, right));
        }





        const int minValidCode = 1;  //Specifies the worst validity code that we will still consider as valid

        readonly IEyeTrackerInfo info;
        readonly IEyetracker tobiiEyeTracker;
        readonly SyncManager syncManager;

        bool findingTimeOffset;
        DateTime startTime;
        TimeSpan timeOffset;

        global::Tobii.Eyetracking.Sdk.Calibration calibration;

    }
}
