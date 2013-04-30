using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Diagnostics;


namespace SensorLib.ThoughtTechnologies
{
    [Serializable]
    class TtlEncoderInfo : ITtlEncoderInfo
    {

        //  Constructors  //


        //  Public Methods //

        public String SerialNumber { get; set; }

        public String ModelName { get; set; }
        public String ModelType { get; set; }

        public int FirmwareVersion { get; set; }
        public int HardwareVersion { get; set; }

        public String ProtocolName { get; set; }
        public String ProtocolType { get; set; }


        //  Overriding Methods  //

        public override bool Equals(object obj)
        {
            TtlEncoderInfo a1 = obj as TtlEncoderInfo;
            if (a1 == null)
                return false;

            return SerialNumber.Equals(a1.SerialNumber);
        }

        public override int GetHashCode()
        {
            return SerialNumber.GetHashCode();
        }


    }

}
