using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minigames.SingeltonClasses;
using Microsoft.Xna.Framework.Input;
using Minigames.InterfaceClasses;

namespace Minigames.PhysicsLogicClasses
{
    class MinigamesPhysics
    {
        
        //
        //fields
        //
        public enum DirectionsEnum { Right = 1, Left, Up, Down };
        public enum EnvironmentEnum { portal_TAG = 1, minigame_TAG};

        //
        //physics classes of other environments
        //
        PuzzlePhysics _puzzle;
        ClickAndHackPhysics _clickAndHack;
        ElectrisPhysics _electris;
        WallDestroyerPhysics _wallDestroyer;

        //constructor
        public MinigamesPhysics()
        {
            _puzzle = new PuzzlePhysics();
            _clickAndHack = new ClickAndHackPhysics();
            _electris = new ElectrisPhysics();
            _wallDestroyer = new WallDestroyerPhysics();
 

        }

        /// <summary>
        /// This function handles all the keyboard events
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="environment"></param>
        public void Move(Microsoft.Xna.Framework.Input.KeyboardState keyState)
        {
            #region log
            if (keyState.GetPressedKeys().Length > 0
                && MINIGAMESDATA.Instance._isLogEnabled
                && MINIGAMESDATA.Instance._isKeyboardEnabled)
            {
                LOG.Instance._logType = LOG.LogTypeEnum.inputLog;
                LOG.Instance._inputDevice = LOG.InputDeviceTypeEnum.keyboardinput;
                LOG.Instance._gameType = MINIGAMESDATA.Instance._currentMiniGame;
                LOG.Instance._message = keyState.GetPressedKeys()[0].ToString();
                MINIGAMESDATA.Instance._log.Message(LOG.Instance.SerializeToString());
                System.Console.WriteLine(LOG.Instance.SerializeToString());
            }
            #endregion
            
            //check the update the emotions based on keyboard inputs
            if (MINIGAMESDATA.Instance._isMinigameRunning)
            {
                //simulating the effect of emotional signals receiving from Emotion Detection Engine
                //puzzle <-> excitemnet
                //brickout <-> fun
                //electris <-> frustration
                #region change_emotion_values
                if (MINIGAMESDATA.Instance._isMotionDebuggEnabled && MINIGAMESDATA.Instance._isKeyboardEnabled)
                {
                    if (keyState.IsKeyDown(Keys.Up))
                    {
                        if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.puzzle_TAG
                            && MINIGAMESDATA.Instance._excitement < 10)
                        {
                            MINIGAMESDATA.Instance._excitement++;
                        }
                        else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.wallDestroyer_TAG
                                && MINIGAMESDATA.Instance._fun < 10)
                        {
                            MINIGAMESDATA.Instance._fun++;
                            if (MINIGAMESDATA.Instance._excitement < 10)
                                MINIGAMESDATA.Instance._excitement++;
                        }

                        else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.electris_TAG
                                && MINIGAMESDATA.Instance._boredom < 10)
                        {
                            MINIGAMESDATA.Instance._boredom++;
                        }
                    }
                    else if (keyState.IsKeyDown(Keys.Down))
                    {
                        if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.puzzle_TAG
                            && MINIGAMESDATA.Instance._excitement > 1)
                        {
                            MINIGAMESDATA.Instance._excitement--;
                        }
                        else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.wallDestroyer_TAG
                                && MINIGAMESDATA.Instance._fun > 1)
                        {
                            MINIGAMESDATA.Instance._fun--;
                            if (MINIGAMESDATA.Instance._excitement > 1)
                                MINIGAMESDATA.Instance._excitement--;
                        }
                        else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.electris_TAG
                                && MINIGAMESDATA.Instance._boredom > 1)
                        {
                            MINIGAMESDATA.Instance._boredom--;
                        }
                    }
                }
                #endregion
            }

            //invoke the update function of the minigames
            if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.puzzle_TAG)
            {
                _puzzle.Update(keyState);   
            }
            else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.electris_TAG)
            {
                _electris.Update(keyState);  
            }
            else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.wallDestroyer_TAG)
            {
                _wallDestroyer.Update(keyState);
            }
            else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.clickAndHack_TAG)
            {
               
            }
        }

		public void UpdateGamepad(GamePadState gamepadState)
		{
            //log
            #region
            string temp = gamepadState.Buttons.ToString();
            string temp2 = gamepadState.DPad.ToString();
            if ((gamepadState.Buttons.ToString() != "{Buttons:None}"
                || gamepadState.DPad.ToString() != "{DPad:None}")
                && MINIGAMESDATA.Instance._isLogEnabled
                )
            {
                LOG.Instance._logType = LOG.LogTypeEnum.inputLog;
                LOG.Instance._inputDevice = LOG.InputDeviceTypeEnum.keyboardinput;
                LOG.Instance._gameType = MINIGAMESDATA.Instance._currentMiniGame;
                if ((gamepadState.Buttons.ToString() != "{Buttons:None}"))
                    LOG.Instance._message = gamepadState.Buttons.ToString();
                else if (gamepadState.DPad.ToString() != "{DPad:None}")
                    LOG.Instance._message = gamepadState.DPad.ToString();
                MINIGAMESDATA.Instance._log.Message(LOG.Instance.SerializeToString());
                System.Console.WriteLine(LOG.Instance.SerializeToString());
            }
            #endregion

			//invoke the update function of the minigames
			if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.puzzle_TAG)
			{
				_puzzle.Update(gamepadState);
			}
			else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.electris_TAG)
			{
				_electris.Update(gamepadState);
			}
			else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.wallDestroyer_TAG)
			{
				_wallDestroyer.Update(gamepadState);
			}
		}
		

        /// <summary>
        /// This function handles all the mouse events
        /// </summary>
        /// <param name="target"></param>
        /// <param name="environment"></param>
        public void Click(Vector2 target, EnvironmentEnum environment)
        {
            if (environment == EnvironmentEnum.portal_TAG)
            {
                //check for the minigames
                MINIGAMESDATA sharedData = MINIGAMESDATA.Instance;

                //adaptationIcon
                Rectangle enableAdaptationRect = new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X + 40,
                        (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y,
                        MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize);
                if (enableAdaptationRect.Contains((int)target.X, (int)target.Y))
                {
                    MINIGAMESDATA.Instance._isAdaptationEnabled = false;
                }
                else
                {
                    Rectangle disableAdaptationRect = new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X + 40,
                        (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y + MINIGAMESDATA.Instance._minigameIconSize,
                        MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize);
                    if (disableAdaptationRect.Contains((int)target.X, (int)target.Y))
                    {
                        //disable logging
                        MINIGAMESDATA.Instance._isAdaptationEnabled = true;
                    }
                }
                
                //user information icon
                Rectangle enableLogRect = new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X,
                        (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y,
                        MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize);
                if (enableLogRect.Contains((int)target.X, (int)target.Y))
                {
                    if (!MINIGAMESDATA.Instance._isLogEnabled)
                    {
                        //PlayerInformation informationForm = new PlayerInformation();
                        //informationForm.Show();
                        MINIGAMESDATA.Instance._isLogEnabled = true;
                    }
                }
                else
                {
                    Rectangle disableLogRect = new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X,
                        (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y + MINIGAMESDATA.Instance._minigameIconSize,
                        MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize);
                    if (disableLogRect.Contains((int)target.X, (int)target.Y) && MINIGAMESDATA.Instance._isLogEnabled)
                    {
                        //disable logging
                        MINIGAMESDATA.Instance._isLogEnabled = false;
                        //save the logged data
                        //MINIGAMESDATA.Instance.Log();
                    }
                }

                //check for nonadaptive puzzles
                int counter = 0;
                foreach (Vector2 position in sharedData._nonadaptivePuzzleMinigamePositions)
                {
                    counter++;
                    Rectangle minigameIconRect = new Rectangle((int)position.X, (int)position.Y,
                    sharedData._minigameIconSize, sharedData._minigameIconSize);
                    if (minigameIconRect.Contains((int)target.X, (int)target.Y))
                    {
                        sharedData._currentMiniGame = MINIGAMESDATA.MinigamesEnum.puzzle_TAG;
                        PUZZLESHAREDDATA.Instance._currentPuzzleType = (PUZZLESHAREDDATA.PuzzleTypeEnum)counter;

                        //handle mouse click event
                        sharedData._currentMinigameRequiredTime = OBJECTS.Instance._minigameTimeMgr.GetTime();
                        sharedData._isMinigameRunning = true;

                        sharedData.Reset(MINIGAMESDATA.MinigamesEnum.puzzle_TAG);
                    }    
                }
                //check for adaptive puzzles
                foreach (Vector2 position in sharedData._adaptivePuzzleMinigamePositions)
                {
                    counter++;
                    Rectangle minigameIconRect = new Rectangle((int)position.X, (int)position.Y,
                    sharedData._minigameIconSize, sharedData._minigameIconSize);
                    if (minigameIconRect.Contains((int)target.X, (int)target.Y))
                    {
                        /*
                        sharedData._currentMiniGame = MINIGAMESDATA.MinigamesEnum.puzzle_TAG;
                        PUZZLESHAREDDATA.Instance._currentPuzzleType = (PUZZLESHAREDDATA.PuzzleTypeEnum)counter;

                        //handle mouse click event
                        sharedData._currentMinigameRequiredTime = OBJECTS.Instance._minigameTimeMgr.GetTime();
                        sharedData._isMinigameRunning = true;

                        sharedData.Reset(MINIGAMESDATA.MinigamesEnum.puzzle_TAG);
                         */
                    }
                }

                //check the nonadaptive ClickAndHack
                foreach (Vector2 position in sharedData._nonadaptiveClickAndHackMinigamePositions)
                {
                    counter++;
                    Rectangle minigameIconRect = new Rectangle((int)position.X, (int)position.Y,
                    sharedData._minigameIconSize, sharedData._minigameIconSize);
                    if (minigameIconRect.Contains((int)target.X, (int)target.Y))
                    {
                        /*
                        sharedData._currentMiniGame = MINIGAMESDATA.MinigamesEnum.clickAndHack_TAG;
                        CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType = (CLICKANDHACKSHAREDDATA.ClickAndHackTypeEnum)counter;

                        //handle mouse click event
                        sharedData._currentMinigameRequiredTime = OBJECTS.Instance._minigameTimeMgr.GetTime();
                        sharedData._isMinigameRunning = true;

                        sharedData.Reset(MINIGAMESDATA.MinigamesEnum.clickAndHack_TAG);
                         */
                    }
                }

                //check the adaptive ClickAndHack
                foreach (Vector2 position in sharedData._adaptiveClickAndHackMinigamePositions)
                {
                    counter++;
                    Rectangle minigameIconRect = new Rectangle((int)position.X, (int)position.Y,
                    sharedData._minigameIconSize, sharedData._minigameIconSize);
                    if (minigameIconRect.Contains((int)target.X, (int)target.Y))
                    {
                        /*
                        sharedData._currentMiniGame = MINIGAMESDATA.MinigamesEnum.clickAndHack_TAG;
                        CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType = (CLICKANDHACKSHAREDDATA.ClickAndHackTypeEnum)counter;

                        //handle mouse click event
                        sharedData._currentMinigameRequiredTime = OBJECTS.Instance._minigameTimeMgr.GetTime();
                        sharedData._isMinigameRunning = true;

                        sharedData.Reset(MINIGAMESDATA.MinigamesEnum.clickAndHack_TAG);
                         */
                    }
                }

                //check the nonadaptive Electris
                foreach (Vector2 position in sharedData._nonadaptiveElectrisMinigamePositions)
                {
                    counter++;
                    Rectangle minigameIconRect = new Rectangle((int)position.X, (int)position.Y,
                    sharedData._minigameIconSize, sharedData._minigameIconSize);
                    if (minigameIconRect.Contains((int)target.X, (int)target.Y))
                    {
                        
                        sharedData._currentMiniGame = MINIGAMESDATA.MinigamesEnum.electris_TAG;
                        ELECTRISSHAREDDATA.Instance._currentElectrisType = (ELECTRISSHAREDDATA.ElectrisTypeEnum)counter;

                        //handle mouse click event
                        sharedData._currentMinigameRequiredTime = OBJECTS.Instance._minigameTimeMgr.GetTime();
                        sharedData._isMinigameRunning = true;

                        sharedData.Reset(MINIGAMESDATA.MinigamesEnum.electris_TAG);
                         
                    }
                }
                //check the adaptive Electris
                foreach (Vector2 position in sharedData._adaptiveElectrisMinigamePositions)
                {
                    counter++;
                    Rectangle minigameIconRect = new Rectangle((int)position.X, (int)position.Y,
                    sharedData._minigameIconSize, sharedData._minigameIconSize);
                    if (minigameIconRect.Contains((int)target.X, (int)target.Y))
                    {
                        /*
                        sharedData._currentMiniGame = MINIGAMESDATA.MinigamesEnum.electris_TAG;
                        ELECTRISSHAREDDATA.Instance._currentElectrisType= (ELECTRISSHAREDDATA.ElectrisTypeEnum)counter;

                        //handle mouse click event
                        sharedData._currentMinigameRequiredTime = OBJECTS.Instance._minigameTimeMgr.GetTime();
                        sharedData._isMinigameRunning = true;

                        sharedData.Reset(MINIGAMESDATA.MinigamesEnum.electris_TAG);
                         */
                    }
                }
                //check the walldestroyer minigame
                foreach (Vector2 position in sharedData._walldestroyerMinigamePositions)
                {
                    counter++;
                    Rectangle minigameIconRect = new Rectangle((int)position.X, (int)position.Y,
                    sharedData._minigameIconSize, sharedData._minigameIconSize);
                    if (minigameIconRect.Contains((int)target.X, (int)target.Y))
                    {
                        sharedData._currentMiniGame = MINIGAMESDATA.MinigamesEnum.wallDestroyer_TAG;
                        WALLDESTROYERSHAREDDATA.Instance._currentWallDestroyerType = (WALLDESTROYERSHAREDDATA.WallDestroyerTypesEnum)counter;

                        //handle mouse click event
                        sharedData._currentMinigameRequiredTime = OBJECTS.Instance._minigameTimeMgr.GetTime();
                        sharedData._isMinigameRunning = true;

                        sharedData.Reset(MINIGAMESDATA.MinigamesEnum.wallDestroyer_TAG);
                    }
                }

            }
            else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.clickAndHack_TAG)
            {
                _clickAndHack.MouseUpdate(target);
            }
            
        }
    }
}
