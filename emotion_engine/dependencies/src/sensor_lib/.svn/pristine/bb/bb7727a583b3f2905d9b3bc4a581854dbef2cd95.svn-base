using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using SensorLib;
using SensorLib.Sensors;


namespace SensorLibBasicExample
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //Create a synchronization context (message pump) for this thread
            //This is because we need a synchronization context to fire event to.
            //This does NOT need to be done when making a windows forms application (because it's already done)
            //This only needs to be done when making a console application
            Form mainForm = new Form();
            mainForm.Load += new EventHandler(mainForm_Load);
            System.Windows.Forms.Application.Run(mainForm);

            //Exit
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            return;
        }


        static void  mainForm_Load(object sender, EventArgs e)
        {
            TestEyeTracker.Start();

            ((Form)sender).Close();
            return;
        }



    }
}
