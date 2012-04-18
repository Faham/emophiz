using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Minigames.SingeltonClasses
{
    public class MINIGAMESDATA
    {
        //static instance to be shared
        private static MINIGAMESDATA instance;

        //log
        public emophiz.Log _log;


        //minigame variables
        public bool _isMinigameRunning;
        public int _currentMinigameRequiredTime;
        public bool _isDataRecorderEnabled;
        public bool _isAdaptationEnabled;

        public Vector2 _userInformationIconPisition;
        public List<Microsoft.Xna.Framework.Vector2> _minigameHeaderPositions;
        public List<Microsoft.Xna.Framework.Vector2> _nonadaptivePuzzleMinigamePositions;
        public List<Microsoft.Xna.Framework.Vector2> _adaptivePuzzleMinigamePositions;

        public List<Microsoft.Xna.Framework.Vector2> _nonadaptiveClickAndHackMinigamePositions;
        public List<Microsoft.Xna.Framework.Vector2> _adaptiveClickAndHackMinigamePositions;

        public List<Microsoft.Xna.Framework.Vector2> _nonadaptiveElectrisMinigamePositions;
        public List<Microsoft.Xna.Framework.Vector2> _adaptiveElectrisMinigamePositions;

        public List<Microsoft.Xna.Framework.Vector2> _walldestroyerMinigamePositions;

        public int _minigameHeaderIconSize;
        public int _minigameIconSize;

        public GameTime _gameTime;

        public enum MinigamesEnum { puzzle_TAG = 1, clickAndHack_TAG, wallDestroyer_TAG, electris_TAG, minigamePortal_TAG};
       
        public MinigamesEnum _currentMiniGame;

        public bool _isLogEnabled;
        //emotions variables
        public bool _isMotionDebuggEnabled;
        public int _defaultEmotionValue;
        public int _maxEmotionValue;
        public int _fun;
        public int _frustration;
        public int _excitement;

        //constructor
        private MINIGAMESDATA()
        {
            _isMinigameRunning = false;
            _currentMinigameRequiredTime = 0;
            _isDataRecorderEnabled = false;
            _gameTime = new GameTime();
            _log = new emophiz.Log("minigames.log");
            _isLogEnabled = false;
            _isAdaptationEnabled = true;
        }

        //public get function
        public static MINIGAMESDATA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MINIGAMESDATA();
                    
                }
                return instance;
            }
        }

        //
        public void Reset(MinigamesEnum minigame)
        {
            //reset emotions
            ResetEmotions(_defaultEmotionValue);

            //invoke the reset function of the minigame
            if (minigame == MinigamesEnum.puzzle_TAG)
                PUZZLESHAREDDATA.Instance.Reset();
            else if (minigame == MinigamesEnum.clickAndHack_TAG)
                CLICKANDHACKSHAREDDATA.Instance.Reset();
            else if (minigame == MinigamesEnum.electris_TAG)
                ELECTRISSHAREDDATA.Instance.Reset();
            else if (minigame == MinigamesEnum.wallDestroyer_TAG)
                WALLDESTROYERSHAREDDATA.Instance.Reset();
        }

        //
        //reset emotions
        //
        public void ResetEmotions(int defaultValue)
        {
            if (MINIGAMESDATA.Instance._isAdaptationEnabled)
            {
                MINIGAMESDATA.Instance._excitement = defaultValue;
                MINIGAMESDATA.Instance._frustration = defaultValue;
                MINIGAMESDATA.Instance._fun = defaultValue;
            }
        }

        //
        //Updates the emotions values
        //
        public void UpdateEmtoions()
        {
            //Updating the following variables
            /*
                public int _fun;
                public int _frustration;
                public int _excitement; 
            */

        }

        //
        //log function
        //
        public bool Log()
        {
            USERINFORMATION info = USERINFORMATION.Instance;
            string logStr = "";
            //user information
            logStr += info._participantID.ToString() + "\t";
            logStr += info._firstName + "\t";
            logStr += info._lastName + "\t";
            logStr += info._age.ToString() + "\t";
            logStr += info._DominantHand + "\t";
            logStr += info._fieldOfStudy + "\t";
            logStr += info._gender + "\t";
            //electris
            logStr += ELECTRISSHAREDDATA.Instance._electrisLogStr;
            //brickout
            logStr += WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr;
            //puzzle
            logStr += PUZZLESHAREDDATA.Instance._puzzleLogStr;

            try
            {
                string filename = @"C:\Users\amin\Documents\Visual Studio 2010\Projects\EmotionalAwareMinigames\Minigames\MinigamesContent\Log\" + USERINFORMATION.Instance._participantID.ToString() + ".txt";
                System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
                logStr += "\n";
                file.WriteLine(logStr);
                file.Close();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
    }
}
