using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SensorLib.Sensors
{

    public delegate void SensorEventHandler(ISensor sensor);
    public delegate void HeartBeatHandler();  //!!Could have amplitude of heartbeat
    
    public delegate void DataAvailableHandler<T>(ISensor<T> sensor, T[] data);
    public delegate void CurrentValueChangedHandler<T>(ISensor<T> sensor, T value);

}
