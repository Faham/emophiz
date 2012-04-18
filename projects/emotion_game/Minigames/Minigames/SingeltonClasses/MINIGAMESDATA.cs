using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Minigames.SingeltonClasses
{
	public class MINIGAMESDATA /*: emophiz.ISensorListener*/
    {
        //static instance to be shared
        private static MINIGAMESDATA instance;

        //log
        public emophiz.Log _log;

        //fuzzy engine resources
        public string _fuzzyEnginResources;

        //input timestamp
        TimeSpan _inputTimeSpan;

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
        public double _fun;
        public double _boredom;
        public double _excitement;
        

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

			m_emotionMonitor = new emophiz.m_frmEmotionMonitor();
			m_emotionMonitor.Show();
			//if (false)
			//    initEmotionProvider();
            
        }

		private emophiz.m_frmEmotionMonitor m_emotionMonitor;

		//private emophiz.SensorProvider m_emotionProvider;
		//private emophiz.Log m_emotionLog = new emophiz.Log("myEmotion.log");

		//private void initEmotionProvider()
		//{
		//    m_emotionProvider = new emophiz.SensorProvider(m_emotionLog);
		//    m_emotionProvider.AddListener(this);
		//    m_emotionProvider.Connect();
		//}

		//bool m_emotionConnected = false;

		//public void OnMessage(emophiz.Message msg, Object value)
		//{
		//    switch (msg)
		//    {
		//        case emophiz.Message.Connecting:
		//            break;
		//        case emophiz.Message.Connected:
		//            m_emotionConnected = true;
		//            break;
		//        case emophiz.Message.Disconnected:
		//            m_emotionConnected = false;
		//            break;
		//        case emophiz.Message.SensorData:
		//            break;
		//        case emophiz.Message.Arousal:
		//            break;
		//        case emophiz.Message.Valence:
		//            break;
		//    }
		//}

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
                MINIGAMESDATA.Instance._boredom = defaultValue;
                MINIGAMESDATA.Instance._fun = defaultValue;
            }
        }

        //
        //Updates the emotions values
        //
        public void UpdateEmtoions()
        {
            //Updating the following variables
			if (m_emotionMonitor.EmotionEngine.Connected)
			{
				_fun = m_emotionMonitor.EmotionEngine.Fun.Transformed / 10.0;
				_boredom = m_emotionMonitor.EmotionEngine.Boredom.Transformed / 10.0;
				_excitement = m_emotionMonitor.EmotionEngine.Excitement.Transformed / 10.0; 
			}
        }

        //
        //log function
        //
        public bool Log()
        {
            USERINFORMATION info = USERINFORMATION.Instance;
            #region minigame_internal_log
            string logStr = "";
            //user information
            logStr += System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:ffff") + "\t";
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
                string filename = @"...\" + USERINFORMATION.Instance._participantID.ToString() + ".txt";
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
            #endregion

            #region emoophiz_participant_log
            try
            {
                using (XmlWriter writer = XmlWriter.Create(@"...\" + USERINFORMATION.Instance._participantID.ToString() + ".xml"))
	            {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Participant");
                    
                    writer.WriteElementString("Date", System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:ffff"));
                    writer.WriteElementString("ID", info._participantID.ToString());
                    writer.WriteElementString("FirstName", info._firstName);
                    writer.WriteElementString("LastName", info._lastName);
                    writer.WriteElementString("Age", info._age.ToString());
                    writer.WriteElementString("Hand", info._DominantHand);
                    writer.WriteElementString("FieldStudy", info._fieldOfStudy);
                    writer.WriteElementString("Gender", info._gender);
                    
	                writer.WriteEndElement();
	                writer.WriteEndDocument();
	            }
            }
            catch (Exception exp)
            {
                try
                {
                    string filename = @"...\log\" + "userInformationError.txt";
                    System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
                    logStr = exp.ToString();
                    file.WriteLine(logStr);
                    file.Close();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    return false;
                }
            }
            #endregion
            return true;
        }
    }
}
