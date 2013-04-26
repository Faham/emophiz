using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using SensorLib;

namespace SensorLib.Util
{

    using Timer = System.Windows.Forms.Timer; //System.Threading.Timer;


    //!!Does not work if you add a column after it has started writing to the file!
    static class Logger
    {
        public delegate float LogDelegate();

        public static void CreateColumn(String header, LogDelegate logDelegate)  //!!logDelegate cannot block!
        {
            lock (loggingLock)
            {
                if (loggingStarted)
                    return;

                headerList.Add(header);
                delegateList.Add(logDelegate);
            }

            return;
        }

        //!!Need to close file!


        public static void StartLogging()
        {
            lock (loggingLock)
            {
                if (loggingStarted)
                    return;

                //Initialization stuff
                {
                    const String extension = ".csv";

                    String logName = String.Format("SensorLog - {0}", DateTime.Now.Date.ToShortDateString()).Replace('/', '_');

                    int copyCount = 1;
                    String logNameTemp = logName;
                    while (File.Exists(logNameTemp + extension))
                    {
                        logNameTemp = logName + '{' + copyCount.ToString() + '}';
                        copyCount++;
                    }


                    textWriter = new StreamWriter(logNameTemp + extension);

                    logTimer = new Timer();
                    logTimer.Interval = 100;  //!!
                    logTimer.Tick += new EventHandler(logTimer_Tick);

                    flushTimer = new Timer();
                    flushTimer.Interval = 1000;  //!!
                    flushTimer.Tick += new EventHandler(flushTimer_Tick);
                }


                StringBuilder builder = new StringBuilder(String.Format("Time (ms){0}", delimiter));

                foreach (String heading in headerList)
                {
                    builder.Append(String.Format("{0}{1}", heading, delimiter));
                }

                //textWriter.WriteLine(DateTime.Now.Date.ToString());
                textWriter.WriteLine(builder.ToString());

                logTimer.Enabled = true;
                flushTimer.Enabled = true;

                loggingStarted = true;

                startTime = DateTime.Now;
            
            }

            return;
        }


        public static void StopLogging()
        {
            lock (loggingLock)
            {
                if (!loggingStarted)
                    return;

                textWriter.WriteLine();
                textWriter.Flush();
                textWriter.Close();

                logTimer.Enabled = false;
                flushTimer.Enabled = false;

                loggingStarted = false;
            }

            return;
        }


        static void logTimer_Tick(object sender, EventArgs e)
        {
            lock (lineQueue)
            {
                float[] values = new float[delegateList.Count() + 1];  //+1 for time column

                float currentTime = (int)((DateTime.Now - startTime).Ticks / 10000);  //1 tick = 100ns;
                values[0] = currentTime;

                for(int i=1; i<delegateList.Count(); i++)  //foreach (LogDelegate logDelegate in delegateList)
                {
                    values[i] = delegateList[i]();
                }

                lineQueue.Enqueue(values);
            }

            return;
        }


        static void flushTimer_Tick(object sender, EventArgs e)
        {
            lock (lineQueue)
            {
                while (lineQueue.Count != 0)
                {
                    float[] values = lineQueue.Dequeue();

                    StringBuilder builder = new StringBuilder(String.Format("{0}{1}", (int)values[0], delimiter));  //Time is first column

                    for (int i = 1; i < values.Length; i++)
                    {
                        builder.Append(String.Format("{0:0.0000}{1}", values[i], delimiter));
                    }

                    textWriter.WriteLine(builder.ToString());
                }
            }

            return;
        }



        static Queue<float[]> lineQueue = new Queue<float[]>();

        static List<String> headerList = new List<String>();
        static List<LogDelegate> delegateList = new List<LogDelegate>();


        static Timer logTimer;
        static Timer flushTimer;

        //static bool initialized = false;
        static TextWriter textWriter;

        static bool loggingStarted = false;
        const int columnWidth = 20;
        const char delimiter = ',';

        static DateTime startTime;
        
        static object loggingLock = new object();
    }
}
