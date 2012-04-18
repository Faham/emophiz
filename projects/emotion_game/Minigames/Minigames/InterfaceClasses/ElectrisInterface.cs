using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minigames.SingeltonClasses;
using C3.XNA;


namespace Minigames.InterfaceClasses
{
    class ElectrisInterface
    {
        //textures
        Texture2D _capacitor;
        Texture2D _diod;
        Texture2D _indicator;
        Texture2D _resistor;
        Texture2D _lamp;
        Texture2D _pattern;
        Texture2D _playground;
        Texture2D _blank;

        //font
        SpriteFont _emotionValueFont;
        public ElectrisInterface()
        {
            //load textures
            _capacitor = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\Capacitor");
            _diod = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\Diod");
            _indicator = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\Indicator");
            _resistor = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\Resistor");
            _lamp = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\Lamp");
            _pattern = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\Pattern");
            _playground = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\ElectrisBackground");
            _blank = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Electris\blank");

            //load font
            _emotionValueFont = OBJECTS.Instance._sharedContent.Load<SpriteFont>(@"Fonts\Font");
        }

        //Draw function
        public void Draw()
        {
            ELECTRISSHAREDDATA obj = ELECTRISSHAREDDATA.Instance;

            //drawbackground
            OBJECTS.Instance._sharedSpriteBatch.Draw(_playground,
                new Rectangle(0, 0, OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width,
                OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);

            //draw components
            for(int i = 0; i < obj._cells.GetLength(0); i++)
	        {
                for (int j = 0; j < obj._cells.GetLength(1); j++)
			    {
                    Texture2D texture = ( obj._cells[i, j]._texture == ELECTRISSHAREDDATA.TextureEnum.blank_TAG ? _blank :
                    (obj._cells[i, j]._texture == ELECTRISSHAREDDATA.TextureEnum.capacitor_TAG ? _capacitor :
                    (obj._cells[i, j]._texture == ELECTRISSHAREDDATA.TextureEnum.diod_TAG ? _diod :
                    (obj._cells[i, j]._texture == ELECTRISSHAREDDATA.TextureEnum.indicator_TAG ? _indicator :
                    (obj._cells[i, j]._texture == ELECTRISSHAREDDATA.TextureEnum.lamp_TAG ? _lamp : _resistor)))));
                    if (texture != _blank)
                        OBJECTS.Instance._sharedSpriteBatch.Draw(texture, new Rectangle(
                            (int)obj._cells[i, j]._position.X, (int)obj._cells[i, j]._position.Y, (int)obj._cellWidth, (int)obj._cellHeight),
                            Color.White);
                    
			    }         
	        }

            //draw emtion value
            if (MINIGAMESDATA.Instance._isMotionDebuggEnabled)
            {
                Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch, new Rectangle(40, 50, 40, 30), Color.Black);
                OBJECTS.Instance._sharedSpriteBatch.DrawString(_emotionValueFont, MINIGAMESDATA.Instance._boredom.ToString(), new Vector2(50, 50), Color.White);
            }
        }
    }
}
