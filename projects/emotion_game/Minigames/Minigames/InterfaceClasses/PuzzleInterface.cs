using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minigames.SingeltonClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace Minigames.InterfaceClasses
{
    class PuzzleInterface
    {
        //
        //textures
        //
        Texture2D[] _puzzle3disksTextures;
        Texture2D[] _puzzle4disksTextures;
        Texture2D[] _puzzle5disksTextures;
        Texture2D[] _puzzle6disksTextures;
        Texture2D[] _puzzle7disksTextures;
        Texture2D[] _puzzle8disksTextures;

        Texture2D _puzzle3disksBackgroundTexture;
        Texture2D _puzzle4disksBackgroundTexture;
        Texture2D _puzzle5disksBackgroundTexture;
        Texture2D _puzzle6disksBackgroundTexture;
        Texture2D _puzzle7disksBackgroundTexture;
        Texture2D _puzzle8disksBackgroundTexture;

        //font
        SpriteFont _emotionValueFont;

        //
        //constructor
        //
        public PuzzleInterface()
        {
            _puzzle3disksTextures = new Texture2D[3];
            _puzzle3disksBackgroundTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\3Disks\background");
            for (int i = 0; i < _puzzle3disksTextures.Length; i++)
            {
                _puzzle3disksTextures[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\3Disks\" + (i+1).ToString());
            }
            _puzzle4disksTextures = new Texture2D[4];
            _puzzle4disksBackgroundTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\4Disks\background");
            for (int i = 0; i < _puzzle4disksTextures.Length; i++)
            {
                _puzzle4disksTextures[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\4Disks\" + (i + 1).ToString());
            }
            _puzzle5disksTextures = new Texture2D[5];
            _puzzle5disksBackgroundTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\5Disks\background");
            for (int i = 0; i < _puzzle5disksTextures.Length; i++)
            {
                _puzzle5disksTextures[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\5Disks\" + (i + 1).ToString());
            }
            _puzzle6disksTextures = new Texture2D[6];
            _puzzle6disksBackgroundTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\6Disks\background");
            for (int i = 0; i < _puzzle6disksTextures.Length; i++)
            {
                _puzzle6disksTextures[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\6Disks\" + (i + 1).ToString());
            }
            _puzzle7disksTextures = new Texture2D[7];
            _puzzle7disksBackgroundTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\7Disks\background");
            for (int i = 0; i < _puzzle7disksTextures.Length; i++)
            {
                _puzzle7disksTextures[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\7Disks\" + (i + 1).ToString());
            }
            _puzzle8disksTextures = new Texture2D[8];
            _puzzle8disksBackgroundTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\8Disks\background");
            for (int i = 0; i < _puzzle8disksTextures.Length; i++)
            {
                _puzzle8disksTextures[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Puzzle\8Disks\" + (i + 1).ToString());
            }

            //load font
            _emotionValueFont = OBJECTS.Instance._sharedContent.Load<SpriteFont>(@"Fonts\Font");

        }

        public void Draw()
        {
            OBJECTS sharedData = OBJECTS.Instance;
            if (PUZZLESHAREDDATA.Instance._currentNumberOfDisks == 3)
            {
                sharedData._sharedSpriteBatch.Draw(_puzzle3disksBackgroundTexture, new Rectangle(0, 0,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);
                for (int i = _puzzle3disksTextures.Length-1; i >= 0 ; i--)
                {
                    sharedData._sharedSpriteBatch.Draw(_puzzle3disksTextures[i],
                        new Rectangle(sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2,
                            sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2,
                            _puzzle3disksTextures[i].Width,
                            _puzzle3disksTextures[i].Height),
                            null,
                            Color.White,
                            (float)((MathHelper.Pi / 180) * PUZZLESHAREDDATA.Instance._currentDegrees[i]),
                            new Vector2(_puzzle3disksTextures[i].Width / 2, _puzzle3disksTextures[i].Height / 2),
                            SpriteEffects.None,
                            0);
                }
            }
            else if (PUZZLESHAREDDATA.Instance._currentNumberOfDisks == 4)
            {
                sharedData._sharedSpriteBatch.Draw(_puzzle4disksBackgroundTexture, new Rectangle(0, 0,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);
                for (int i = _puzzle4disksTextures.Length - 1; i >= 0; i--)
                {
                    sharedData._sharedSpriteBatch.Draw(_puzzle4disksTextures[i],
                        new Rectangle(sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2,
                            sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2,
                            _puzzle4disksTextures[i].Width,
                            _puzzle4disksTextures[i].Height),
                            null,
                            Color.White,
                            (float)((MathHelper.Pi / 180) * PUZZLESHAREDDATA.Instance._currentDegrees[i]),
                            new Vector2(_puzzle4disksTextures[i].Width / 2, _puzzle4disksTextures[i].Height / 2),
                            SpriteEffects.None,
                            0);
                }
            }
            else if (PUZZLESHAREDDATA.Instance._currentNumberOfDisks == 5)
            {
                sharedData._sharedSpriteBatch.Draw(_puzzle5disksBackgroundTexture, new Rectangle(0, 0,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);
                for (int i = _puzzle5disksTextures.Length - 1; i >= 0; i--)
                {
                    sharedData._sharedSpriteBatch.Draw(_puzzle5disksTextures[i],
                        new Rectangle(sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2,
                            sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2,
                            _puzzle5disksTextures[i].Width,
                            _puzzle5disksTextures[i].Height),
                            null,
                            Color.White,
                            (float)((MathHelper.Pi / 180) * PUZZLESHAREDDATA.Instance._currentDegrees[i]),
                            new Vector2(_puzzle5disksTextures[i].Width / 2, _puzzle5disksTextures[i].Height / 2),
                            SpriteEffects.None,
                            0);
                }
            }
            else if (PUZZLESHAREDDATA.Instance._currentNumberOfDisks == 6)
            {
                sharedData._sharedSpriteBatch.Draw(_puzzle6disksBackgroundTexture, new Rectangle(0, 0,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);
                for (int i = _puzzle6disksTextures.Length - 1; i >= 0; i--)
                {
                    sharedData._sharedSpriteBatch.Draw(_puzzle6disksTextures[i],
                        new Rectangle(sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2,
                            sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2,
                            _puzzle6disksTextures[i].Width,
                            _puzzle6disksTextures[i].Height),
                            null,
                            Color.White,
                            (float)((MathHelper.Pi / 180) * PUZZLESHAREDDATA.Instance._currentDegrees[i]),
                            new Vector2(_puzzle6disksTextures[i].Width / 2, _puzzle6disksTextures[i].Height / 2),
                            SpriteEffects.None,
                            0);
                }
            }
            else if (PUZZLESHAREDDATA.Instance._currentNumberOfDisks == 7)
            {
                sharedData._sharedSpriteBatch.Draw(_puzzle3disksBackgroundTexture, new Rectangle(0, 0,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);
                for (int i = _puzzle7disksTextures.Length - 1; i >= 0; i--)
                {
                    sharedData._sharedSpriteBatch.Draw(_puzzle7disksTextures[i],
                        new Rectangle(sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2,
                            sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2,
                            _puzzle7disksTextures[i].Width,
                            _puzzle7disksTextures[i].Height),
                            null,
                            Color.White,
                            (float)((MathHelper.Pi / 180) * PUZZLESHAREDDATA.Instance._currentDegrees[i]),
                            new Vector2(_puzzle7disksTextures[i].Width / 2, _puzzle7disksTextures[i].Height / 2),
                            SpriteEffects.None,
                            0);
                }
            }
            else if (PUZZLESHAREDDATA.Instance._currentNumberOfDisks == 8)
            {
                sharedData._sharedSpriteBatch.Draw(_puzzle3disksBackgroundTexture, new Rectangle(0, 0,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                    sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);
                for (int i = _puzzle8disksTextures.Length - 1; i >= 0; i--)
                {
                    sharedData._sharedSpriteBatch.Draw(_puzzle8disksTextures[i],
                        new Rectangle(sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2,
                            sharedData._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2,
                            _puzzle8disksTextures[i].Width,
                            _puzzle8disksTextures[i].Height),
                            null,
                            Color.White,
                            (float)((MathHelper.Pi / 180) * PUZZLESHAREDDATA.Instance._currentDegrees[i]),
                            new Vector2(_puzzle8disksTextures[i].Width / 2, _puzzle8disksTextures[i].Height / 2),
                            SpriteEffects.None,
                            0);
                }
            }


            //draw emotion value
            if (MINIGAMESDATA.Instance._isMotionDebuggEnabled)
            {
                Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch, new Rectangle(40, 50, 40, 30), Color.Black);
                OBJECTS.Instance._sharedSpriteBatch.DrawString(_emotionValueFont, MINIGAMESDATA.Instance._excitement.ToString(), new Vector2(50, 50), Color.White);
            }

        }
    }
}
