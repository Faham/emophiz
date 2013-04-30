using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Util
{
    [Serializable]
    public class Point2D
    {
        public Point2D()
        {
            this.x = 0.0f;
            this.y = 0.0f;
        }

        public Point2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float X { get { return x; } }
        public float Y { get { return y; } }

        readonly float x;
        readonly float y;
    }
}
