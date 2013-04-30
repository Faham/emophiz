using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;


using SensorLib.Sensors;
using SensorLib.Sensors.Inputs;
using SensorLib.Util;


namespace SensorLib.Mindset
{

    using Timer = System.Windows.Forms.Timer;

    //Mindset by Neurosky


    //NOTE: For simplicity sake, all methods in this class should use the session thread except for the constructor.
    //      This makes synchronization easier (public methods and event handlers just need to switch to session thread) and it also makes the class thread-safe!

    //NOTE: Mindset connects but does not return data if you have gotten the DeviceControlPluginInfoManager from WMEncoder in the Windows Media Encoder SDK.  I guess the Mindset drivers cannot handle being asked for device controls.

    //!! Is actually a fixed sample rate!
    public class MindsetDevice : Device<IMindsetInfo, Sample<BrainData>>
    {


        #region Getting Devices

        static MindsetDevice()
        {
            //Get version of the ThinkGear Communications Driver (TGCD) library
            libraryVersion = ThinkGear.TG_GetDriverVersion();

            return;
        }

        public static MindsetDevice Connect(int comNumber)
        {
            return Connect("COM" + comNumber);
        }

        public static MindsetDevice Connect(String portName)
        {
            return new MindsetDevice(portName);
        }

        public static int LibraryVersion { get { return libraryVersion; } }

        static readonly int libraryVersion;


        //!!public static event Action<IMindsetInfo> MindsetAdded;
        //!!public static event Action<IMindsetInfo> MindsetRemoved;

        #endregion


        //////////////////////
        // Instance Methods //
        //////////////////////


        protected MindsetDevice(String portName) : base()
        {
            
            //Initialize readonly variables
            badReads = 0;
            battery = 0.0f;
            threadControl = new Control();
            threadControl.CreateControl();
            samples = 0;

            MindsetInfo info = new MindsetInfo();
            info.PortName = portName;
            this.info = info;

            startTime = DateTime.Now;
            //SynchronizationContext synchContext = new SynchronizationContext();

            thread = new ApplicationThread(ThreadInit, ThreadDeInit, 5000);
            thread.Start();

            return;
        }



        public event Action<MindsetDevice, float> BatteryPercentageChanged;
        public event Action<MindsetDevice, float> SignalQualityChanged;

        public float BatteryPercentage { get { lock (devicePropertyLock) { return battery; } } }
        public float SignalQuality { get { lock (devicePropertyLock) { return signalQuality; } } }
        public int BaudRate { get { return ThinkGear.BAUD_9600; } }

        public override IMindsetInfo Info { get { return info; } }

        public ISensor<Sample<BrainData>> CreateSensor(String name, bool queueData)
        {
            SampleTimeShiftProcessor<BrainData> processor = new SampleTimeShiftProcessor<BrainData>(startTime);
            Sensor<Sample<BrainData>> sensor = new Sensor<Sample<BrainData>>(processor, new QueueInput<Sample<BrainData>>(samplesPerSecond), name, queueData);
            processor.Sensor = sensor;
            base.CreateSensor(sensor);
            return sensor;
        }

        /*
        //!!
        public ISensor<Sample<float>> CreateSensor(String name, BrainBand band, bool queueData)
        {
            Sensor<Sample<float>> sensor = new Sensor<Sample<float>>(new NoneProcessor<BrainData>(), new QueueInput<BrainData>(samplesPerSecond), name, queueData);
            base.CreateSensor(sensor);
            return sensor;
        }
        */
        
        
        protected override void DeviceDispose()
        {

            
            //Exit anyone invoking the threadControl control
            threadControl.Dispose();

            //Wait for thread to finish
            thread.Join();


            return;
        }

        protected override void DeviceAddSensor(Sensor<Sample<BrainData>> sensor) { return; }
        protected override void DeviceRemoveSensor(Sensor<Sample<BrainData>> sensor) { return; }
        protected override void DeviceFirstSensorAdded() { return; }
        protected override void DeviceLastSensorRemoved() { return; }

        protected override void DeviceReceivedData(Sensor<Sample<BrainData>> sensor, Sample<BrainData>[] data)
        {
            ((QueueInput<Sample<BrainData>>)sensor.Input).AddData(data);
            return;
        }






        
        void ThreadInit()
        {
            Thread.Sleep(500);

            //Set up the data timer
            dataTimer = new Timer();
            dataTimer.Interval = dataPollMilliseconds;
            dataTimer.Tick += new EventHandler(dataTimer_Tick);

            //Get a connection object handle
            id = ThinkGear.TG_GetNewConnectionId();
            if (id < 0)
                throw new Exception("Could not get an id from the library.");

            //Create variable for return values
            int ret;

            //Set the stream log to off
            ret = ThinkGear.TG_SetStreamLog(id, null);
            if (ret != 0)
                throw new Exception("Could not turn off stream log.");

            //Set the data log to off
            ret = ThinkGear.TG_SetDataLog(id, null);
            if (ret != 0)
                throw new Exception("Could not turn off data log.");

            //Connect to the headset
            ret = ThinkGear.TG_Connect(id, info.PortName, BaudRate, ThinkGear.STREAM_PACKETS);
            if (ret != 0)
                throw new Exception("Could not connect to the device.");


            //Start up the data timer
            dataTimer.Start();

            return;
        }


        void ThreadDeInit()
        {
            
            //Stop grabbing data
            dataTimer.Stop();

            //Disconnect from the headset and free up memory
            ThinkGear.TG_Disconnect(id);
            ThinkGear.TG_FreeConnection(id);

            return;
        }


        void dataTimer_Tick(object sender, EventArgs e)
        {

            int packetCount;

            //Check if this is our first time getting data
            if (samples == 0)
            {
                //Get the last packet of the stream
                packetCount = ThinkGear.TG_ReadPackets(id, -1);
                if (packetCount == 0)
                    return;

                poorSignal = 200;

                //Register the time of our first packet
                timeOffset = DateTime.Now - startTime;
                samples++;
            }
            else
            {
                //Read in the next data packet from the stream
                packetCount = ThinkGear.TG_ReadPackets(id, 1);
                while (packetCount > 0)
                {
                    Debug.Assert(packetCount == 1);

                    Sample<BrainData> data = new Sample<BrainData>(oldData);
                    badReads = 0;

                    data.Time = TimeSpan.FromSeconds(samples / (float)samplesPerSecond) + timeOffset;

                    
                    lock (devicePropertyLock)
                    {
                        // Signal quality
                        float lastSignalQuality = signalQuality;
                        GetValue(id, ThinkGear.DATA_POOR_SIGNAL, ref poorSignal);
                        signalQuality = 100 - (poorSignal / 2.0f);

                        if (SignalQualityChanged != null && signalQuality != lastSignalQuality)
                            threadControl.BeginInvoke(SignalQualityChanged, this, signalQuality);

                        // Battery percentage
                        float lastBattery = battery;
                        GetValue(id, ThinkGear.DATA_BATTERY, ref battery);

                        if (BatteryPercentageChanged != null && battery != lastBattery)
                            threadControl.BeginInvoke(BatteryPercentageChanged, this, battery);
                    }

                    data.Value.signal = signalQuality;

                    GetValue(id, ThinkGear.DATA_ATTENTION, ref data.Value.attention);
                    GetValue(id, ThinkGear.DATA_MEDITATION, ref data.Value.meditation);

                    GetValue(id, ThinkGear.DATA_RAW, ref data.Value.raw);

                    GetValue(id, ThinkGear.DATA_ALPHA1, ref data.Value.alpha1);
                    GetValue(id, ThinkGear.DATA_ALPHA2, ref data.Value.alpha2);
                    GetValue(id, ThinkGear.DATA_BETA1, ref data.Value.beta1);
                    GetValue(id, ThinkGear.DATA_BETA2, ref data.Value.beta2);
                    GetValue(id, ThinkGear.DATA_DELTA, ref data.Value.delta);
                    GetValue(id, ThinkGear.DATA_GAMMA1, ref data.Value.gamma1);
                    GetValue(id, ThinkGear.DATA_GAMMA2, ref data.Value.gamma2);
                    GetValue(id, ThinkGear.DATA_THETA, ref data.Value.theta);


                    //Send data to the inputs
                    ReceivedData(new Sample<BrainData>[] { data });


                    oldData = data;
                    samples++;

                    //Read in the next data packet from the stream
                    packetCount = ThinkGear.TG_ReadPackets(id, 1);
                }
            
            }


            if (packetCount < 0)
            {
                if (packetCount == -2)
                {
                    
                    badReads++;

                    //Check if we can get data
                    if (badReads > maxBadReads)
                    {
                        try
                        {
                            threadControl.Invoke(new Action(Drop));
                        }
                        catch
                        {
                            //do nothing because we probably disposed the control
                        }
                    }
                }
                else
                {
                    try
                    {
                        threadControl.Invoke(new Action(Drop));
                    }
                    catch
                    {
                        //do nothing because we probably disposed the control
                    }
                }
            }

            return;
        }


        
        
        void GetValue(int id, int dataType, ref float variable)
        {
            float[] values = ReadData(id, dataType, 1);
            Debug.Assert(values.Length == 0 || values.Length == 1);
            if (values.Length == 0)
                return;

            variable = values[values.Length - 1];
            return;
        }
        

        float[] ReadData(int id, int dataType, int packetCount)
        {
            //Check if we have an updated value for the data type
            int ret = ThinkGear.TG_GetValueStatus(id, dataType);
            if(ret == 0)
                return new float[0];
            
            //Read the data
            float[] data = new float[packetCount];
            for (int i = 0; i < packetCount; i++)
            {
                data[i] = ThinkGear.TG_GetValue(id, dataType);
            }

            return data;
        }


        
        const float samplesPerSecond = 512;

        const int maxBadReads = 20;  //maximum number of reads with no data before we decide we are disconnected
        const int dataPollMilliseconds = 100; //number of milliseconds to wait before asking for data

        readonly ApplicationThread thread;
        readonly Control threadControl;

        readonly IMindsetInfo info;
        readonly object devicePropertyLock = new object();

        readonly DateTime startTime;


        Timer dataTimer;
        
        int id;
        Sample<BrainData> oldData = new Sample<BrainData>(TimeSpan.Zero, new BrainData());
        
        float battery;
        float signalQuality;
        float poorSignal;
        int badReads;  //Counts how many reads have resulted in no data


        //Stuff needed to calculate the time each data element arrived
        TimeSpan timeOffset;
        int samples;


    }
}
