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

namespace MinigamesLibrary
{
    public class MinigamesXMLStructure
    {
        public Vector2 LIBRARY_userInformationIconPosition;
        public List<Vector2> LIBRARY_minigameHeaderPosition;
        public List<Vector2> LIBRARY_nonadaptivePuzzlePositions;
        public List<Vector2> LIBRARY_adaptivePuzzlePositions;
        public List<Vector2> LIBRARY_nonadaptiveClickAndHackPositions;
        public List<Vector2> LIBRARY_adaptiveClickAndHackPositions;
        public List<Vector2> LIBRARY_nonadaptiveElectrisPositions;
        public List<Vector2> LIBRARY_adaptiveElectrisPositions;
        public List<Vector2> LIBRARY_walldestroyerMinigamePositions;
        public string LIBRARY_fuzzyEnginResources;
        
        

        public int LIBRARY_minigameHeaderIconSize;
        public int LIBRARY_minigameIconSize;
        public List<int> LIBRARY_puzzle_NumberOfDisksLst;
        public List<PuzzleDisk> LIBRARY_puzzleDisks;
        public double LIBRARY_puzzleDefaultRotationSpeed;
        public int LIBRARY_puzzleHintPosition;
        public int LIBRARY_puzzleHintSize;
        public int LIBRARY_puzzleHintGapDelay;
        public int LIBRARY_puzzleHintDelay;

        public List<Node> LIBRARY_nodePositions;
        public Vector2 LIBRARY_smallNodeSize;
        public Vector2 LIBRARY_largeNodeSize;
        public Vector2 LIBRARY_hackBtnSize;

        public List<Point> LIBRARY_tetrisCellPositions;
        public int LIBRARY_numberOfRows;
        public int LIBRARY_numberOfColumns;
        public int LIBRARY_cellWidth;
        public int LIBRARY_cellHeight;
        public List<int> LIBRARY_electrisPattern;
        public int LIBRARY_fallingDelay;

        public List<Vector2> LIBRARY_brickPositions;
        public Vector2 LIBRARY_brickSize;
        public int LIBRARY_numberOfBricksInARow;
        public int LIBRARY_ballPosition;
        public int LIBRARY_ballSize;
        public int LIBRARY_boardPosition;
        public Vector2 LIBRARY_boardSize;
        public int LIBRARY_boardSpeed;
        public int LIBRARY_ballSpeed;
        public int LIBRARY_compassHandleAngleSpeed;
        public int LIBRARY_defaultBoardSpeed;

        public int LIBRARY_defaultEmotionValue;
        public int LIBRARY_maxEmotionValue;
        public int LIBRARY_isMotionDebugEnabled;
    }
}
