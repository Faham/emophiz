using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Minigames.InterfaceClasses;
using Minigames.SingeltonClasses;
using Minigames.XMLClasses;
using Minigames.PhysicsLogicClasses;
using Minigames.UtilityClasses;

namespace Minigames
{
    public class MiniGamesCore : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        //mouse and keyboard object
        MouseState _mouse;
        KeyboardState _keyboard;

        //minigame physics object
        MinigamesPhysics _physics;

        //interface object
        PortalInterface _interface;

        //XML loader object
        MinigamesXMLLoader _minigamesXMLLoader;

        public MiniGamesCore()
        {
            #region scene-configuration
            //set the scene
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //ask for the current screen resolution
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.PreferredBackBufferWidth = 1280;
            //_graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            //make it full screen
            _graphics.IsFullScreen = true;
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0/60.0);

            #endregion
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //set the shared instance
            OBJECTS.Instance._sharedContent = Content;
            OBJECTS.Instance._sharedGraphicDeviceMgr = _graphics;
            OBJECTS.Instance._sharedSpriteBatch = _spriteBatch;

            //initialize the xml loader class
            _minigamesXMLLoader = new MinigamesXMLLoader();
            _minigamesXMLLoader.Load(@"XMLFiles\MinigamesSetting");

            //set the minigamedata shared data class
            MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;

            //initialize the interface object
            _interface = new PortalInterface();

            //initialize the physics object
            _physics = new MinigamesPhysics();
            
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //update the shared gametime
            MINIGAMESDATA.Instance._gameTime = gameTime;

            //update emotions
            MINIGAMESDATA.Instance.UpdateEmtoions();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _mouse = Mouse.GetState();
            _keyboard = Keyboard.GetState();

            #region mouse_click
            
            //check mouse event
            if (_mouse.LeftButton == ButtonState.Pressed)
            {
                if (!MINIGAMESDATA.Instance._isMinigameRunning)
                    _physics.Click(new Vector2(_mouse.X, _mouse.Y), MinigamesPhysics.EnvironmentEnum.portal_TAG);
                else if (MINIGAMESDATA.Instance._isMinigameRunning)
                    _physics.Click(new Vector2(_mouse.X, _mouse.Y), MinigamesPhysics.EnvironmentEnum.minigame_TAG);
            }
            #endregion

            #region Handle_KeyboardEvents
            //handle keyboard events
            if (MINIGAMESDATA.Instance._isMinigameRunning)
            {
                _physics.Move(_keyboard);
            }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _interface.Draw();
            
            base.Draw(gameTime);
        }
    }
}
