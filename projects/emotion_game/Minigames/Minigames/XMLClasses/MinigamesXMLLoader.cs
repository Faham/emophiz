using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinigamesLibrary;
using Minigames.SingeltonClasses;
using Microsoft.Xna.Framework;

namespace Minigames.XMLClasses
{
    class MinigamesXMLLoader
    {
        MinigamesXMLStructure _minigamesSharedContent;
        
        public void Load(string fileName)
        {
            try
            {
                _minigamesSharedContent = OBJECTS.Instance._sharedContent.Load<MinigamesXMLStructure>(fileName);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error in loading the XML file");
                System.Console.WriteLine(e.ToString());
            }

            //load portal interface and general values
            MINIGAMESDATA.Instance._userInformationIconPisition = _minigamesSharedContent.LIBRARY_userInformationIconPosition;
            MINIGAMESDATA.Instance._minigameHeaderPositions = _minigamesSharedContent.LIBRARY_minigameHeaderPosition;
            MINIGAMESDATA.Instance._nonadaptivePuzzleMinigamePositions = _minigamesSharedContent.LIBRARY_nonadaptivePuzzlePositions;
            MINIGAMESDATA.Instance._adaptivePuzzleMinigamePositions = _minigamesSharedContent.LIBRARY_adaptivePuzzlePositions;
            MINIGAMESDATA.Instance._nonadaptiveClickAndHackMinigamePositions = _minigamesSharedContent.LIBRARY_nonadaptiveClickAndHackPositions;
            MINIGAMESDATA.Instance._adaptiveClickAndHackMinigamePositions = _minigamesSharedContent.LIBRARY_adaptiveClickAndHackPositions;
            MINIGAMESDATA.Instance._minigameHeaderIconSize = _minigamesSharedContent.LIBRARY_minigameHeaderIconSize;
            MINIGAMESDATA.Instance._minigameIconSize = _minigamesSharedContent.LIBRARY_minigameIconSize;
            MINIGAMESDATA.Instance._nonadaptiveElectrisMinigamePositions = _minigamesSharedContent.LIBRARY_nonadaptiveElectrisPositions;
            MINIGAMESDATA.Instance._adaptiveElectrisMinigamePositions = _minigamesSharedContent.LIBRARY_adaptiveElectrisPositions;
            MINIGAMESDATA.Instance._walldestroyerMinigamePositions = _minigamesSharedContent.LIBRARY_walldestroyerMinigamePositions;

            //loads emotional values
            MINIGAMESDATA.Instance._defaultEmotionValue = _minigamesSharedContent.LIBRARY_defaultEmotionValue;
            MINIGAMESDATA.Instance._maxEmotionValue = _minigamesSharedContent.LIBRARY_maxEmotionValue;
            if (_minigamesSharedContent.LIBRARY_isMotionDebugEnabled == 1)
                MINIGAMESDATA.Instance._isMotionDebuggEnabled = true;
            else
                MINIGAMESDATA.Instance._isMotionDebuggEnabled = false;

            //loads puzzle data
            PUZZLESHAREDDATA.Instance._puzzleTypes = _minigamesSharedContent.LIBRARY_puzzle_NumberOfDisksLst;
            PUZZLESHAREDDATA.Instance._puzzleDisksDegrees = _minigamesSharedContent.LIBRARY_puzzleDisks;
            PUZZLESHAREDDATA.Instance._defaultSpeed = _minigamesSharedContent.LIBRARY_puzzleDefaultRotationSpeed;
            PUZZLESHAREDDATA.Instance._puzzleHintPosition = _minigamesSharedContent.LIBRARY_puzzleHintPosition;
            PUZZLESHAREDDATA.Instance._puzzleHintSize = _minigamesSharedContent.LIBRARY_puzzleHintSize;
            PUZZLESHAREDDATA.Instance._puzzleHintGapDelay = _minigamesSharedContent.LIBRARY_puzzleHintGapDelay * 60;
            PUZZLESHAREDDATA.Instance._puzzleHintDelay = _minigamesSharedContent.LIBRARY_puzzleHintDelay * 60;


            //loads hackAComputer data
            CLICKANDHACKSHAREDDATA.Instance._nodePositions = _minigamesSharedContent.LIBRARY_nodePositions;
            CLICKANDHACKSHAREDDATA.Instance._smallNodeSize = _minigamesSharedContent.LIBRARY_smallNodeSize;
            CLICKANDHACKSHAREDDATA.Instance._largeNodeSize = _minigamesSharedContent.LIBRARY_largeNodeSize;
            CLICKANDHACKSHAREDDATA.Instance._hackBtnSize = _minigamesSharedContent.LIBRARY_hackBtnSize;

            //loads electris data
            ELECTRISSHAREDDATA.Instance._cellPositioins = _minigamesSharedContent.LIBRARY_tetrisCellPositions;
            ELECTRISSHAREDDATA.Instance._numberOfRows = _minigamesSharedContent.LIBRARY_numberOfRows;
            ELECTRISSHAREDDATA.Instance._numberOfColumns = _minigamesSharedContent.LIBRARY_numberOfColumns;
            ELECTRISSHAREDDATA.Instance._cellWidth = _minigamesSharedContent.LIBRARY_cellWidth;
            ELECTRISSHAREDDATA.Instance._cellHeight = _minigamesSharedContent.LIBRARY_cellHeight;
            ELECTRISSHAREDDATA.Instance._pattern = _minigamesSharedContent.LIBRARY_electrisPattern;
            ELECTRISSHAREDDATA.Instance._fallingDelay = _minigamesSharedContent.LIBRARY_fallingDelay;
            

            //loads brickout data
            WALLDESTROYERSHAREDDATA.Instance._brickPositionsLst = _minigamesSharedContent.LIBRARY_brickPositions;
            WALLDESTROYERSHAREDDATA.Instance._brickSize = _minigamesSharedContent.LIBRARY_brickSize;
            WALLDESTROYERSHAREDDATA.Instance._numberOfBricksInARow = _minigamesSharedContent.LIBRARY_numberOfBricksInARow;
            WALLDESTROYERSHAREDDATA.Instance._ballHorizontalPosition = _minigamesSharedContent.LIBRARY_ballPosition;
            WALLDESTROYERSHAREDDATA.Instance._ballSize = _minigamesSharedContent.LIBRARY_ballSize;
            WALLDESTROYERSHAREDDATA.Instance._boardSize = _minigamesSharedContent.LIBRARY_boardSize;
            WALLDESTROYERSHAREDDATA.Instance._boardHorizontalposition = _minigamesSharedContent.LIBRARY_boardPosition;
            WALLDESTROYERSHAREDDATA.Instance._defaultBoardSpeed = _minigamesSharedContent.LIBRARY_boardSpeed;
            WALLDESTROYERSHAREDDATA.Instance._defaultBallSpeed = _minigamesSharedContent.LIBRARY_ballSpeed;
            WALLDESTROYERSHAREDDATA.Instance._compassPosition = new Vector2(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2, OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2);
            WALLDESTROYERSHAREDDATA.Instance._handlePosition = new Vector2(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2, OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height / 2);
            WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleSpeed = _minigamesSharedContent.LIBRARY_compassHandleAngleSpeed;
        }
    }
}
