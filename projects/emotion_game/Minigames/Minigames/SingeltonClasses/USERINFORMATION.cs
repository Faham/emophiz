using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minigames.SingeltonClasses
{
    class USERINFORMATION
    {
        //static variale to be shared
        private static USERINFORMATION instance;

        //class variables
        public int _participantID;
        public string _firstName;
        public string _lastName;
        public int _age;
        public string _fieldOfStudy;
        public string _gender;
        public string _DominantHand;


        //private constructor
        //constructor
        private USERINFORMATION()
        {
        }

        //public get function
        public static USERINFORMATION Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new USERINFORMATION();
                }
                return instance;
            }
        }

    }
}
