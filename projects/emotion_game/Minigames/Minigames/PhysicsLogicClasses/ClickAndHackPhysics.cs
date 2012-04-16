using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minigames.SingeltonClasses;

namespace Minigames.PhysicsLogicClasses
{
    class ClickAndHackPhysics
    {   
        //
        //Update function
        //
        public void MouseUpdate(Vector2 target)
        {
            #region Detect_Taget_Click
            CLICKANDHACKSHAREDDATA clickAndHackSharedData = CLICKANDHACKSHAREDDATA.Instance;
            
            Rectangle nodeRect = new Rectangle((int)clickAndHackSharedData._currentNodePositions[clickAndHackSharedData._currentNodeIndex].X,
                (int)clickAndHackSharedData._currentNodePositions[clickAndHackSharedData._currentNodeIndex].Y,
                (int)clickAndHackSharedData._smallNodeSize.X, (int)clickAndHackSharedData._smallNodeSize.Y);
             
            
            Rectangle hackBtnRect = new Rectangle((int)(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2 -
            CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.X / 2),
            (int)(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height -
            CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.Y),
            (int)CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.X,
            (int)CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.Y);

            if (hackBtnRect.Contains(new Point((int)target.X, (int)target.Y)) && !CLICKANDHACKSHAREDDATA.Instance._isHackBtnClicked)
            {
                CLICKANDHACKSHAREDDATA.Instance._isHackBtnClicked = true;
            }
            else if (nodeRect.Contains(new Point((int)target.X, (int)target.Y)) && CLICKANDHACKSHAREDDATA.Instance._isHackBtnClicked)
            {
                clickAndHackSharedData._currentNodeIndex++;
                CLICKANDHACKSHAREDDATA.Instance._isHackBtnClicked = false;
            }
            
            #endregion


            //check the game status
            if (IsGameFinished())
            {
                CLICKANDHACKSHAREDDATA.Instance._currentGameResult = true;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
            }
        }

        public void KeyboardUdpate(KeyboardState key)
        {
            #region Detect_Quit_Hit
            if (key.IsKeyDown(Keys.Escape))
            {
                //check for the game result
                CLICKANDHACKSHAREDDATA.Instance._currentGameResult = false;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
            }
            #endregion
        }

        //
        //checks the status of the minigame
        //
        public bool IsGameFinished()
        {
            if (CLICKANDHACKSHAREDDATA.Instance._currentNodeIndex == CLICKANDHACKSHAREDDATA.Instance._currentNumberOfNodes)
                return true;
            else
                return false;
        }
    }
}
