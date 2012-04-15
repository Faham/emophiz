using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Util
{
    [Serializable]
    public class Point3D
    {
        public Point3D()
        {
            this.x = 0.0f;
            this.y = 0.0f;
            this.z = 0.0f;
        }

        public Point3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float X { get { return x; } }
        public float Y { get { return y; } }
        public float Z { get { return z; } }

        readonly float x;
        readonly float y;
        readonly float z;
    }
}
