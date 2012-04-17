using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minigames.SingeltonClasses
{
    public class LOG
    {
        //static instance to be shared
        private static LOG instance;

        //log types
        public enum LogTypeEnum { inputLog = 0, informationLog };
        
        //input types
        public enum InputDeviceTypeEnum { mouseInput = 0, keyboardinput };

        //other variables
        public LogTypeEnum _logType;
        public InputDeviceTypeEnum _inputDevice;
        public MINIGAMESDATA.MinigamesEnum _gameType;
        public string _message;

        //constructor
        private LOG()
        {
            
        }

        //public get function
        public static LOG Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LOG();
                }
                return instance;
            }
        }

        //serialize method
        public string SerializeToString()
        {
            string res = "";
            res += _logType.ToString() + "\t";
            if (_logType == LogTypeEnum.inputLog)
                res += _inputDevice.ToString() + "\t";
            res += _gameType.ToString() + "\t";
            res += _message + "\n";
            return res;
        }
    }
}
