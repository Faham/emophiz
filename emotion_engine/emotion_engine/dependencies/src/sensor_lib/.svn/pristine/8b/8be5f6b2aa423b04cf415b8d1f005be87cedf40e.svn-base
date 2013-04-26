using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SensorLib.Util;

namespace SensorLib.Tobii
{
    [Serializable]
    public class EyeData
    {
        public EyeData() : this(new Eye(), new Eye())
        { }

        public EyeData(Eye left_, Eye right_)
        {
            left = left_;
            right = right_;
            return;
        }

        public readonly Eye left;
        public readonly Eye right;

        [Serializable]
        public class Eye
        {
            public Eye() : this(false, 4, 0.0f, new Point3D(), new Point2D())
            { }

            public Eye(bool valid_, int validityCode_, float pupilDiameter_, Point3D eyePos_, Point2D gazePos_)
            {
                valid = valid_;
                validityCode = validityCode_;
                pupilDiameter = pupilDiameter_;
                eyePos = eyePos_;
                gazePos = gazePos_;
                return;
            }

            public readonly bool valid;
            public readonly int validityCode;
            public readonly float pupilDiameter;
            /// <summary>
            /// Gives the eye position between 0 and 1
            /// </summary>
            public readonly Point3D eyePos;
            public readonly Point2D gazePos;
        }

    }
}
