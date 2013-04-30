using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Diagnostics;


namespace SensorLib.Xna
{
    [Serializable]
    class AudioDeviceInfo : IAudioDeviceInfo
    {

        public AudioDeviceInfo(Microsoft.Xna.Framework.Audio.Microphone microphone)
        {
            this.name = microphone.Name;
            return;
        }

        public String Name { get { return name; } }


        readonly String name;
    }

}
