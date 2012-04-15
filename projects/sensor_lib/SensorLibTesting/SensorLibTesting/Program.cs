using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using SensorLib;
using SensorLib.Base;
using SensorLib.Sensors;


namespace SensorLibTesting
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


        static void mainForm_Load(object sender, EventArgs e)
        {
            try
            {
                ((Form)sender).Visible = false;
                DeviceTests.TestNormalOperation(new MindsetTestDevice());
                Console.WriteLine("\n\nTest completed successfully.");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("\n\nTest was NOT successful!");
            }

            ((Form)sender).Close();
            return;
        }



    }
}
