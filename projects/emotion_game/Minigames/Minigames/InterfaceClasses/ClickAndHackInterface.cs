using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minigames.SingeltonClasses;

namespace Minigames.InterfaceClasses
{
    class ClickAndHackInterface
    {
        //
        //textures
        //
        Texture2D _nodeTexture;
        Texture2D _hackBtnClicked;
        Texture2D _hackBtn;

        //
        //Constructor
        //
        public ClickAndHackInterface()
        {
            //load textures
            _nodeTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\ClickAndHack\node");
            _hackBtn = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\ClickAndHack\HackBtn");
            _hackBtnClicked = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\ClickAndHack\HackBtn_Clicked");
        }

        //
        //Drawing function
        //
        public void Draw()
        {
            //draw here
            //remember to add the hack button to the physics class of this mninigame
            if (CLICKANDHACKSHAREDDATA.Instance._isHackBtnClicked)
            {
                OBJECTS.Instance._sharedSpriteBatch.Draw(_nodeTexture,
                    new Rectangle((int)CLICKANDHACKSHAREDDATA.Instance._currentNodePositions[CLICKANDHACKSHAREDDATA.Instance._currentNodeIndex].X,
                        (int)CLICKANDHACKSHAREDDATA.Instance._currentNodePositions[CLICKANDHACKSHAREDDATA.Instance._currentNodeIndex].Y,
                        (int)CLICKANDHACKSHAREDDATA.Instance._smallNodeSize.X, (int)CLICKANDHACKSHAREDDATA.Instance._smallNodeSize.Y), Color.White);

                OBJECTS.Instance._sharedSpriteBatch.Draw(_hackBtnClicked, new Rectangle((int)(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2 -
                    CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.X / 2),
                    (int)(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height -
                    CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.Y),
                    (int)CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.X,
                    (int)CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.Y), Color.White);
            }
            else
            {
                OBJECTS.Instance._sharedSpriteBatch.Draw(_hackBtn, new Rectangle((int)(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2 -
                CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.X / 2),
                (int)(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height -
                CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.Y),
                (int)CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.X,
                (int)CLICKANDHACKSHAREDDATA.Instance._hackBtnSize.Y), Color.White);
            }
        }
    }
}