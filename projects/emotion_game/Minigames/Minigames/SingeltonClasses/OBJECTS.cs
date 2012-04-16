using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minigames.UtilityClasses;

namespace Minigames.SingeltonClasses
{
    class OBJECTS
    {
        //static instance to be shared
        private static OBJECTS instance;

        //public fields
        public GraphicsDeviceManager _sharedGraphicDeviceMgr;
        public ContentManager _sharedContent;
        public SpriteBatch _sharedSpriteBatch;
        public MinigameTimeManager _minigameTimeMgr;

        //constructor
        private OBJECTS()
        {
            _minigameTimeMgr = new MinigameTimeManager();
        }

        //public get function
        public static OBJECTS Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OBJECTS();
                }
                return instance;
            }
        }
    }
}
