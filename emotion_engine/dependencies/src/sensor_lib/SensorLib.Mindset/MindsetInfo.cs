using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Mindset
{

    [Serializable]
    class MindsetInfo : IMindsetInfo
    {

        //  Public Methods //

        public string PortName { get; set; }


        //  Overriding Methods  //

        public override bool Equals(object obj)
        {
            MindsetInfo a1 = obj as MindsetInfo;
            if (a1 == null)
                return false;

            return PortName.Equals(a1.PortName);
        }

        public override int GetHashCode()
        {
            return PortName.GetHashCode();
        }


    }
}
