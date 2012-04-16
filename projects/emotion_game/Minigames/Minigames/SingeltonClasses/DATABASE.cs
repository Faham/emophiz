using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minigames.SingeltonClasses
{
    class DATABASE
    {
        //static instance to be shared
        private static DATABASE instance;

        //public variables

        //public get function
        public static DATABASE Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DATABASE();
                }
                return instance;
            }
        }
    }
}
