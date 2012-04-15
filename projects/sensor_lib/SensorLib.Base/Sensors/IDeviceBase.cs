using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors
{

    public interface IDevice : IDisposable
    {

        /// <summary>
        /// Is fired when device is disposed by user.
        /// </summary>
        event Action<IDevice> Disconnected;

        /// <summary>
        /// Is fired when device was forced to be disconnected due to error or other disconnection.
        /// </summary>
        event Action<IDevice> Dropped;

    }

}
