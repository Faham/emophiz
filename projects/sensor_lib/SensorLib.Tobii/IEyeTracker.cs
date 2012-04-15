using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;


namespace SensorLib.Tobii
{
    public interface IEyeTracker : SensorLib.Sensors.IDevice
    {

        //void Calibrate();
        //void Calibrate(bool isRecalibrating);
        //void InterruptCalibration();

        //void DrawCalibPlot(System.Windows.Forms.Panel panel_, System.ComponentModel.ComponentResourceManager resources);


        //void SaveCalibrationFile(String name);
        //void LoadCalibrationFile(String name);

        /*
        void StartCalibration();
        void StopCalibration();
        void AddCalibrationPoint(Point2D point);
        void LoadCalibration(String path);
        void SaveCalibration(String path);
        */

        IEyeSensor CreateSensor(String name, bool queueData);
        
        void Calibrate();
        Image ViewCalibration();
        void SaveCalibration(String path);
        void LoadCalibration(String path);

        IEyeTrackerInfo Info { get; }

    }
}
