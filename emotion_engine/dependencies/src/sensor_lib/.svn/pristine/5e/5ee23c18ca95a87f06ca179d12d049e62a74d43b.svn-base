using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

using SensorLib.Sensors.Inputs;
using SensorLib.Util;

namespace SensorLib.Sensors
{
    //A useful sensor is something that takes data, processes it, and then makes it available.
    //!!Can make queueData be changed after constructor too
    public class Sensor<T> : Sensor, ISensor<T>
    {
        
        public Sensor(IProcessor<T> processor, IInput<T> input, String name, bool queueData) : base(name, processor, input)
        {
            
            this.processor = processor;
            this.input = input;
            this.queueData = queueData;

            outputDataQ = new TSQueue<T>();

            CurrentValue = default(T);


            callingThreadControl = new Control();
            callingThreadControl.CreateControl();

            //Thread stuff
            threadExit = false;
            dataThread = new Thread(new ThreadStart(ProcessDataThreadStart));
            dataThread.IsBackground = true;
            dataThread.Start();

            return;
        }
        
        ~Sensor()
        {
            Dispose();
            return;
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                //Signal processing thread to exit
                threadExit = true;

                //Allow user to leave
                outputDataQ.SignalExit();

                //Allow processing thread to exit
                input.Disconnect();

                //Wait for processing thread to exit
                dataThread.Join(1000);

                // Stop sensor
                Stop();

                //Don't dispose when instance is destroyed
                GC.SuppressFinalize(this);
            }

            base.Dispose();
            return;
        }


        public event CurrentValueChangedHandler<T> CurrentValueChanged;

        public event DataAvailableHandler<T> DataAvailable;
        
        public bool QueueData { get { return queueData; } }

        public T CurrentValue { get; private set; }

        public IInput<T> Input { get { return input; } }
        public IProcessor<T> Processor { get { return processor; } }


        // Called from front
        public T[] GetData(int maxSamples)
        {
            if (!queueData)
                throw new InvalidOperationException("Sensor is not set up to queue data");

            return outputDataQ.Dequeue(maxSamples);
        }
        

        protected override void OnStop()
        {
            // Clear the output queue
            outputDataQ.Clear();
            
            base.OnStop();
            return;
        }


        void ProcessDataThreadStart()
        {
            while (!threadExit)
            {
                /*
                // Wait for channel to start
                Monitor.Enter(dataThread);
                {
                    if (!IsStarted)
                        Monitor.Wait(dataThread);
                }
                Monitor.Exit(dataThread);
                */
                lock (isStartedLock)
                {
                    // Check if we are supposed to exit
                    if (threadExit || !IsStarted)
                        continue;

                    // Grab data from queue
                    T[] data = input.GetData(MAX_SAMPLES);

                    // Check again if we are supposed to exit
                    if (threadExit || !IsStarted)
                        continue;

                    // Process the data
                    T[] processedData = processor.ProcessData(data);

                    // Check if we still have any data
                    if (processedData.Length == 0)
                        continue;

                    // Set the last value as our current value
                    T newCurrentValue = processedData[processedData.Length - 1];
                    bool currentValueChanged = !newCurrentValue.Equals(CurrentValue);
                    CurrentValue = newCurrentValue;


                    // Queue the data
                    if (queueData)
                    {
                        if (outputDataQ.Enqueue(processedData) == false)
                            Console.WriteLine("Error: Queue Overflow!");
                    }

                    try
                    {
                        // Fire events
                        if (currentValueChanged)
                            callingThreadControl.BeginInvoke(new Action<T, DateTime>(CurrentValueChangedEventFire), CurrentValue, StartTime);

                        if (DataAvailable != null)
                            callingThreadControl.BeginInvoke(new Action<T[], DateTime>(DataAvailableEventFire), processedData, StartTime);
                    }
                    catch
                    {
                        // do nothing because we probably disposed the control
                    }
                }
            }

            return;
        }

        /// <summary>
        /// Makes sure event does not fire after sensor is stopped.
        /// </summary>
        /// <param name="value"></param>
        void CurrentValueChangedEventFire(T value, DateTime startTime)
        {
            // Check if sensor is started or if the event is leftover from a previous start
            if (!IsStarted || StartTime != startTime)
                return;

            if (CurrentValueChanged != null)
                CurrentValueChanged(this, value);
            return;
        }

        /// <summary>
        /// Makes sure event does not fire after sensor is stopped.
        /// </summary>
        /// <param name="data"></param>
        void DataAvailableEventFire(T[] data, DateTime startTime)
        {
            // Check if sensor is started or if the event is leftover from a previous start
            if (!IsStarted || StartTime != startTime)
                return;

            if (DataAvailable != null)
                DataAvailable(this, data);
            return;
        }


        
        const int MAX_SAMPLES = 255;

        readonly bool queueData;    //is true if the user wants to be able to grab the data values this instance receives
                                    //this should be false if the user does not want the data so that the queue does not overflow

        
        readonly IProcessor<T> processor;
        readonly IInput<T> input;

        readonly TSQueue<T> outputDataQ;

        readonly Control callingThreadControl;

        readonly Thread dataThread;
        bool threadExit;

    }
}
