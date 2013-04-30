using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tobii.Eyetracking.Sdk;

namespace SDK_Basic_Eyetracking_Sample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize Tobii SDK eyetracking library
            Library.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
