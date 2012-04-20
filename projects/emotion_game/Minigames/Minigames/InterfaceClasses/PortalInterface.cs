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
    class PortalInterface
    {
        //
        //other interface classes
        //
        PuzzleInterface _puzzle;
        ClickAndHackInterface _clickAndHack;
        ElectrisInterface _electris;
        WallDestroyerInterface _wallDestroyer;

        //textures
        Texture2D _backgroundTexture;
        Texture2D _startLogTexture;
        Texture2D _stopLogTexture;
        Texture2D _enableAdaptationTexture;
        Texture2D _disableAdaptationTexture;

        Texture2D[] _headerTexturesArray;
        Texture2D[] _nonadaptivePuzzleTexturesArray;
        Texture2D[] _nonadaptiveElectrisTexturesArray;
        Texture2D[] _nonadaptiveBrickoutTexturesArray;

        public PortalInterface()
        {
            //
            //loading other interface classes
            //
            _puzzle = new PuzzleInterface();
            _clickAndHack = new ClickAndHackInterface();
            _electris = new ElectrisInterface();
            _wallDestroyer = new WallDestroyerInterface();
            //
            //loading other textures
            //
            _backgroundTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\background3");
            _startLogTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\startLog");
            _stopLogTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\stopLog");
            _enableAdaptationTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\enableAdaptation");
            _disableAdaptationTexture = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\disableAdaptation");

            _headerTexturesArray = new Texture2D[3];
            _nonadaptiveBrickoutTexturesArray = new Texture2D[3];
            _nonadaptiveElectrisTexturesArray = new Texture2D[3];
            _nonadaptivePuzzleTexturesArray = new Texture2D[6];
            
            _headerTexturesArray[0] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\PuzzleIcons\PuzzleHeader");
            _headerTexturesArray[1] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\ElectrisIcons\ElectrisHeader");
            _headerTexturesArray[2] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\BrickOutIcons\BrickOutHeader");

            _nonadaptiveBrickoutTexturesArray[0] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\BrickOutIcons\brickout2");
            _nonadaptiveBrickoutTexturesArray[1] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\BrickOutIcons\brickout4");
            _nonadaptiveBrickoutTexturesArray[2] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\BrickOutIcons\brickout6");
            for (int i = 0; i < _nonadaptiveElectrisTexturesArray.Length; i++)
            {
                _nonadaptiveElectrisTexturesArray[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\ElectrisIcons\electris" + (i+1).ToString());
            }
            for (int i = 0; i < _nonadaptivePuzzleTexturesArray.Length; i++)
            {
                _nonadaptivePuzzleTexturesArray[i] = OBJECTS.Instance._sharedContent.Load<Texture2D>(@"Pictures\Portal\PuzzleIcons\puzzle" + (i + 3).ToString());
            }
        }

        /// <summary>
        /// Drawing function
        /// </summary>
        public void Draw()
        {
            //begin drawing
            OBJECTS.Instance._sharedSpriteBatch.Begin();
            if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG)
            {
                #region Draw_Menu

                //draw the background
                OBJECTS.Instance._sharedSpriteBatch.Draw(_backgroundTexture,
                    new Rectangle(0, 0, _backgroundTexture.Width,
                        OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height), Color.White);
                //draw adaptation icon
                if (MINIGAMESDATA.Instance._isAdaptationEnabled)
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_enableAdaptationTexture,
                        new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X + 40,
                            (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y,
                            MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize),
                            Color.White);
                else
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_disableAdaptationTexture,
                        new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X + 40,
                            (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y + MINIGAMESDATA.Instance._minigameIconSize,
                            MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize),
                            Color.White); 

                //draw the user information icon
                if (!MINIGAMESDATA.Instance._isLogEnabled)
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_startLogTexture,
                        new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X,
                            (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y,
                            MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize),
                            Color.White); 
                else
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_stopLogTexture,
                        new Rectangle((int)MINIGAMESDATA.Instance._userInformationIconPisition.X,
                            (int)MINIGAMESDATA.Instance._userInformationIconPisition.Y + MINIGAMESDATA.Instance._minigameIconSize,
                            MINIGAMESDATA.Instance._minigameIconSize, MINIGAMESDATA.Instance._minigameIconSize),
                            Color.White); 
                
                //draw headers
                OBJECTS.Instance._sharedSpriteBatch.Draw(_headerTexturesArray[0],
                            new Rectangle((int)MINIGAMESDATA.Instance._minigameHeaderPositions[0].X, (int)MINIGAMESDATA.Instance._minigameHeaderPositions[0].Y,
                                (int)MINIGAMESDATA.Instance._minigameHeaderIconSize,
                                (int)MINIGAMESDATA.Instance._minigameHeaderIconSize),
                            Color.White);
                OBJECTS.Instance._sharedSpriteBatch.Draw(_headerTexturesArray[1],
                            new Rectangle((int)MINIGAMESDATA.Instance._minigameHeaderPositions[2].X, (int)MINIGAMESDATA.Instance._minigameHeaderPositions[2].Y,
                                (int)MINIGAMESDATA.Instance._minigameHeaderIconSize,
                                (int)MINIGAMESDATA.Instance._minigameHeaderIconSize),
                            Color.White);
                OBJECTS.Instance._sharedSpriteBatch.Draw(_headerTexturesArray[2],
                            new Rectangle((int)MINIGAMESDATA.Instance._minigameHeaderPositions[3].X, (int)MINIGAMESDATA.Instance._minigameHeaderPositions[3].Y,
                                (int)MINIGAMESDATA.Instance._minigameHeaderIconSize,
                                (int)MINIGAMESDATA.Instance._minigameHeaderIconSize),
                            Color.White);

                //draw puzzle
                int counter = 0;
                foreach (Vector2 pos in MINIGAMESDATA.Instance._nonadaptivePuzzleMinigamePositions)
                {
                    if (counter != 4 && counter != 5)
                    {
                        OBJECTS.Instance._sharedSpriteBatch.Draw(_nonadaptivePuzzleTexturesArray[counter],
                            new Rectangle((int)pos.X, (int)pos.Y,
                        (int)MINIGAMESDATA.Instance._minigameIconSize,
                        (int)MINIGAMESDATA.Instance._minigameIconSize),
                        Color.White);
                    }
                    counter++;
                }
                /*
                foreach (Vector2 pos in MINIGAMESDATA.Instance._adaptivePuzzleMinigamePositions)
                {
                    Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch,
                    new Rectangle((int)pos.X, (int)pos.Y,
                    (int)MINIGAMESDATA.Instance._minigameIconSize,
                    (int)MINIGAMESDATA.Instance._minigameIconSize), Color.Gray);
                }
                 
                //draw click and hack computer
                foreach (Vector2 pos in MINIGAMESDATA.Instance._nonadaptiveClickAndHackMinigamePositions)
                {
                    Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch,
                    new Rectangle((int)pos.X, (int)pos.Y,
                    (int)MINIGAMESDATA.Instance._minigameIconSize,
                    (int)MINIGAMESDATA.Instance._minigameIconSize), Color.Gray);
                }
                foreach (Vector2 pos in MINIGAMESDATA.Instance._adaptiveClickAndHackMinigamePositions)
                {
                    Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch,
                    new Rectangle((int)pos.X, (int)pos.Y,
                    (int)MINIGAMESDATA.Instance._minigameIconSize,
                    (int)MINIGAMESDATA.Instance._minigameIconSize), Color.Gray);
                }
                 */
                //draw electris
                counter = 0;
                foreach (Vector2 pos in MINIGAMESDATA.Instance._nonadaptiveElectrisMinigamePositions)
                {
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_nonadaptiveElectrisTexturesArray[counter],
                        new Rectangle((int)pos.X, (int)pos.Y,
                    (int)MINIGAMESDATA.Instance._minigameIconSize,
                    (int)MINIGAMESDATA.Instance._minigameIconSize),
                    Color.White);
                    counter++;
                }
                /*
                foreach (Vector2 pos in MINIGAMESDATA.Instance._adaptiveElectrisMinigamePositions)
                {
                    Primitives2D.FillRectangle(OBJECTS.Instance._sharedSpriteBatch,
                    new Rectangle((int)pos.X, (int)pos.Y,
                    (int)MINIGAMESDATA.Instance._minigameIconSize,
                    (int)MINIGAMESDATA.Instance._minigameIconSize), Color.Gray);
                }
                 */
                //draw walldestroyer
                counter = 0;
                foreach (Vector2 pos in MINIGAMESDATA.Instance._walldestroyerMinigamePositions)
                {
                    OBJECTS.Instance._sharedSpriteBatch.Draw(_nonadaptiveBrickoutTexturesArray[counter],
                        new Rectangle((int)pos.X, (int)pos.Y,
                    (int)MINIGAMESDATA.Instance._minigameIconSize,
                    (int)MINIGAMESDATA.Instance._minigameIconSize),
                    Color.White);
                    counter++;
                }
                #endregion
            }
            else if (MINIGAMESDATA.Instance._isMinigameRunning)
            {
                if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.puzzle_TAG)
                {
                    _puzzle.Draw();
                }
                else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.clickAndHack_TAG)
                {
                    _clickAndHack.Draw();
                }
                else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.electris_TAG)
                    _electris.Draw();
                else if (MINIGAMESDATA.Instance._currentMiniGame == MINIGAMESDATA.MinigamesEnum.wallDestroyer_TAG)
                    _wallDestroyer.Draw();

            }
            //finish drawing
            OBJECTS.Instance._sharedSpriteBatch.End();
        }
    }
}
