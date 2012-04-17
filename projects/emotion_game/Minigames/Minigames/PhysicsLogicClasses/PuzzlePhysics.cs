﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Minigames.SingeltonClasses;

namespace Minigames.PhysicsLogicClasses
{
    class PuzzlePhysics
    {   

        public PuzzlePhysics()
        {
            
        }

        public void Update(KeyboardState key)
        {

            #region log
            /*
            LOG.Instance._logType = LOG.LogTypeEnum.informationLog;
            LOG.Instance._gameType = MINIGAMESDATA.Instance._currentMiniGame;
            //message = current number of rings +  current active ring + position of current ring + current speed + game result;
            LOG.Instance._message = PUZZLESHAREDDATA.Instance._currentNumberOfDisks.ToString() + "\t";
            LOG.Instance._message += PUZZLESHAREDDATA.Instance._currentActiveRing.ToString() + "\t";
            if (PUZZLESHAREDDATA.Instance._currentActiveRing != -1)
                LOG.Instance._message += PUZZLESHAREDDATA.Instance._currentDegrees[PUZZLESHAREDDATA.Instance._currentActiveRing - 1].ToString() + "\t";
            else
                LOG.Instance._message += "not available yet\t";
            LOG.Instance._message += PUZZLESHAREDDATA.Instance._currentSpeed.ToString() + "\t";
            LOG.Instance._message += PUZZLESHAREDDATA.Instance._currentGameResult.ToString();
            MINIGAMESDATA.Instance._log.Message(LOG.Instance.SerializeToString());
            System.Console.WriteLine(LOG.Instance.SerializeToString());
            */ 
            #endregion

            #region update_rotation_speed
            int emotion = MINIGAMESDATA.Instance._maxEmotionValue - MINIGAMESDATA.Instance._excitement;
            if (emotion > 0)
                PUZZLESHAREDDATA.Instance._currentSpeed = (double)PUZZLESHAREDDATA.Instance._defaultSpeed * emotion / MINIGAMESDATA.Instance._maxEmotionValue;
            else
                PUZZLESHAREDDATA.Instance._currentSpeed= 0.1;//remember to talk about it!             
            #endregion

            #region Detect_Number_Hit
            if (key.IsKeyDown(Keys.D1) && !PUZZLESHAREDDATA.Instance._isActive[0])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[0] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 1;
            }
            else if (key.IsKeyDown(Keys.D2) && !PUZZLESHAREDDATA.Instance._isActive[1])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[1] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 2;
            }
            else if (key.IsKeyDown(Keys.D3) && !PUZZLESHAREDDATA.Instance._isActive[2])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[2] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 3;
            }
            else if (key.IsKeyDown(Keys.D4) && PUZZLESHAREDDATA.Instance._currentNumberOfDisks >= 4 && !PUZZLESHAREDDATA.Instance._isActive[3])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[3] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 4;
            }
            else if (key.IsKeyDown(Keys.D5) && PUZZLESHAREDDATA.Instance._currentNumberOfDisks >= 5 && !PUZZLESHAREDDATA.Instance._isActive[4])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[4] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 5;
            }
            else if (key.IsKeyDown(Keys.D6) && PUZZLESHAREDDATA.Instance._currentNumberOfDisks >= 6 && !PUZZLESHAREDDATA.Instance._isActive[5])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[5] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 6;
            }
            else if (key.IsKeyDown(Keys.D7) && PUZZLESHAREDDATA.Instance._currentNumberOfDisks >= 7 && !PUZZLESHAREDDATA.Instance._isActive[6])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[6] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 7;
            }
            else if (key.IsKeyDown(Keys.D8) && PUZZLESHAREDDATA.Instance._currentNumberOfDisks == 8 && !PUZZLESHAREDDATA.Instance._isActive[7])
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    PUZZLESHAREDDATA.Instance._isActive[i] = false;
                }
                PUZZLESHAREDDATA.Instance._isActive[7] = true;
                PUZZLESHAREDDATA.Instance._currentActiveRing = 8;
            }
            #endregion

            #region Detect_ArrowKey_Hit
            if (key.IsKeyDown(Keys.Right))
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    if (PUZZLESHAREDDATA.Instance._isActive[i])
                        PUZZLESHAREDDATA.Instance._currentDegrees[i] += PUZZLESHAREDDATA.Instance._currentSpeed;
                }
            }
            else if (key.IsKeyDown(Keys.Left))
            {
                for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
                {
                    if (PUZZLESHAREDDATA.Instance._isActive[i])
                        PUZZLESHAREDDATA.Instance._currentDegrees[i] -= PUZZLESHAREDDATA.Instance._currentSpeed;
                }
            } 
            #endregion

            #region Detect_Quit_Hit
            if (key.IsKeyDown(Keys.Escape))
            {
                //check for the game result
                PUZZLESHAREDDATA.Instance._currentGameResult = false;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
                //log
                TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                PUZZLESHAREDDATA.Instance._puzzleLogStr += timeStamp.TotalSeconds.ToString() + "\t";
                PUZZLESHAREDDATA.Instance._puzzleLogStr += PUZZLESHAREDDATA.Instance._currentGameResult ? "1" : "0";
                PUZZLESHAREDDATA.Instance._puzzleLogStr += "\t";
            }
            #endregion

            #region Detect_Game_Finishing
            //check for the game result
            if (IsGameFinished())
            {
                PUZZLESHAREDDATA.Instance._currentGameResult = true;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
                //log
                TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                PUZZLESHAREDDATA.Instance._puzzleLogStr += timeStamp.TotalSeconds.ToString() + "\t";
                PUZZLESHAREDDATA.Instance._puzzleLogStr += PUZZLESHAREDDATA.Instance._currentGameResult ? "1" : "0";
                PUZZLESHAREDDATA.Instance._puzzleLogStr += "\t";
            }
            #endregion

        }

        public bool IsGameFinished()
        {
            for (int i = 0; i < PUZZLESHAREDDATA.Instance._currentNumberOfDisks; i++)
            {
                if ((Math.Abs(PUZZLESHAREDDATA.Instance._currentDegrees[i]) % 360) > PUZZLESHAREDDATA.Instance._degreeOfFreedom
                || (Math.Abs(PUZZLESHAREDDATA.Instance._currentDegrees[i]) % 360) < -1 *PUZZLESHAREDDATA.Instance._degreeOfFreedom)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
