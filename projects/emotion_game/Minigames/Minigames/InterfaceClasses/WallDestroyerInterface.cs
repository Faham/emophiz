using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minigames.SingeltonClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Minigames.InterfaceClasses
{
    class WallDestroyerInterface
    {
        //Textures
        Texture2D _brickTexture;
        Texture2D _ballTexture;
        Texture2D _boardTexture;
        Texture2D _compassTexture;
        Texture2D _compassHandleTexture;
        Texture2D _background;
        Vector2 _compassHandleAnchorPoint;

        //font
        SpriteFont _emotionValueFont;

        //constructor
        public WallDestroyerInterface()
        {
            _brickTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\WallDestroyer\brick");
            _ballTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\WallDestroyer\ball");
            _compassTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\WallDestroyer\compass");
            _compassHandleTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\WallDestroyer\handle");
            _background = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\WallDestroyer\background");
            _compassHandleAnchorPoint = new Vector2(0, _compassHandleTexture.Height/2);
            
            //load font
            _emotionValueFont = OBJECTS.Instance._sharedContent.Load<SpriteFont>(@"Fonts\Font");
        }
        //draw function
        public void Draw()
        {
            //an instance of the shared object
            WALLDESTROYERSHAREDDATA obj = WALLDESTROYERSHAREDDATA.Instance;
            
            //draw background
            OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Clear(Color.Green);
            /*
            OBJECTS.Instance._sharedSpriteBatch.Draw(_background, new Rectangle(0, 0,
                OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height),
                Color.White);
            */
            //draw bricks
            foreach (WALLDESTROYERSHAREDDATA.Brick brick in obj._brickLst)
            {
                if (!brick._isHit)
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_brickTexture,
                        new Rectangle((int)brick._position.X, (int)brick._position.Y, (int)obj._brickSize.X, (int)obj._brickSize.Y), Color.White);
            }
            /*
            for (int i = 0; i < obj._currentNumberOfBricks; i++)
			{
			    if (!obj._brickLst[i]._isHit)
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_brickTexture, 
                        new Rectangle((int)obj._brickLst[i]._position.X, (int)obj._brickLst[i]._position.Y, (int)obj._brickSize.X, (int)obj._brickSize.Y), Color.White);
			}
            */
            
            //draw the ball
            if (WALLDESTROYERSHAREDDATA.Instance._isBallActive)
                OBJECTS.Instance._sharedSpriteBatch.Draw(_ballTexture,
                        new Rectangle((int)obj._ballPosition.X, (int)obj._ballPosition.Y, obj._ballSize, obj._ballSize), Color.White);
            else
                OBJECTS.Instance._sharedSpriteBatch.Draw(_ballTexture,
                        new Rectangle((int)obj._ballPosition.X, (int)obj._ballPosition.Y, obj._ballSize, obj._ballSize), Color.Gray);
            //draw board
            Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch, new Rectangle((int)(obj._boardPosition.X - 10), (int)obj._boardPosition.Y, (int)obj._boardSize.X, (int)obj._boardSize.Y), Color.Yellow);

            //draw the compass and handle
            if (!WALLDESTROYERSHAREDDATA.Instance._isShoot)
            {
                OBJECTS.Instance._sharedSpriteBatch.Draw(_compassTexture, new Rectangle((int)(WALLDESTROYERSHAREDDATA.Instance._compassPosition.X - _compassTexture.Width / 2), (int)WALLDESTROYERSHAREDDATA.Instance._compassPosition.Y, _compassTexture.Width, _compassTexture.Height), Color.White);
                OBJECTS.Instance._sharedSpriteBatch.Draw(_compassHandleTexture, new Rectangle((int)(WALLDESTROYERSHAREDDATA.Instance._handlePosition.X), (int)WALLDESTROYERSHAREDDATA.Instance._handlePosition.Y + _compassTexture.Height, _compassHandleTexture.Width, _compassHandleTexture.Height), null, Color.White, WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleRadian, _compassHandleAnchorPoint, SpriteEffects.None, 0);
                Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch, new Rectangle((int)(WALLDESTROYERSHAREDDATA.Instance._handlePosition.X) - 10, (int)WALLDESTROYERSHAREDDATA.Instance._handlePosition.Y + _compassTexture.Height, 30, 30), Color.Black);
                OBJECTS.Instance._sharedSpriteBatch.DrawString(_emotionValueFont, (WALLDESTROYERSHAREDDATA.Instance._numberOfAvailableBalls - WALLDESTROYERSHAREDDATA.Instance._numberOfBalls + 1).ToString(),
                    new Vector2((int)(WALLDESTROYERSHAREDDATA.Instance._handlePosition.X), (int)WALLDESTROYERSHAREDDATA.Instance._handlePosition.Y + _compassTexture.Height), Color.White);
            }

            //debug collision detection
            //Primitives2D.DrawRectangle(OBJECTS.Instance._sharedSpriteBatch, new Rectangle((int)WALLDESTROYERSHAREDDATA.Instance._nextBallPosition.X, (int)WALLDESTROYERSHAREDDATA.Instance._nextBallPosition.Y, WALLDESTROYERSHAREDDATA.Instance._ballSize, WALLDESTROYERSHAREDDATA.Instance._ballSize), Color.Red);

            

            //draw emotion value
            if (MINIGAMESDATA.Instance._isMotionDebuggEnabled)
            {
                Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch, new Rectangle(40, 50, 40, 30), Color.Black);
                OBJECTS.Instance._sharedSpriteBatch.DrawString(_emotionValueFont, MINIGAMESDATA.Instance._fun.ToString(), new Vector2(50, 50), Color.White);
            }
        }
    }
}
