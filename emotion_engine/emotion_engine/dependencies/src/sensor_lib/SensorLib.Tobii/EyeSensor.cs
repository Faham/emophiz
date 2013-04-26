using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Threading;

using Tobii.Eyetracking.Sdk;
using Tobii.Eyetracking.Sdk.Exceptions;
using Tobii.Eyetracking.Sdk.Time;

using SensorLib.Sensors.Inputs;
using SensorLib.Util;
using SensorLib.Sensors;

namespace SensorLib.Tobii
{


    class EyeSensor : Sensor<Sample<EyeData>>, IEyeSensor
    {

        public EyeSensor(EyeTracker eyeTracker, float samplesPerSecond, String name, bool queueData) : base(new SampleTimeShiftProcessor<EyeData>(eyeTracker.StartTime), new QueueInput<Sample<EyeData>>(samplesPerSecond), name, queueData)
        {
            this.device = eyeTracker;
            ((SampleTimeShiftProcessor<EyeData>)this.Processor).Sensor = this;
            return;
        }


        readonly EyeTracker device;
    }

}
