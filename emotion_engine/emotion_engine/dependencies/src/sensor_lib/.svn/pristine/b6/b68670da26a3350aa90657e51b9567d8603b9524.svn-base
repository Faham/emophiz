using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Util
{
    
    public class Pair<T, U>
    {
        public Pair() { }

        public Pair(Pair<T, U> pair)
        {
            this.First = pair.First;
            this.Second = pair.Second;
            return;
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
            return;
        }

        public void CopyTo(Pair<T, U> pair)
        {
            pair.First = First;
            pair.Second = Second;
            return;
        }

        public T First { get; set; }
        public U Second { get; set; }
    }
}
