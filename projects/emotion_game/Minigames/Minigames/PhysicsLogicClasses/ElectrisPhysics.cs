﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minigames.SingeltonClasses;
using Microsoft.Xna.Framework.Input;


namespace Minigames.PhysicsLogicClasses
{
    
    
    //electris physics class
    class ElectrisPhysics
    {
        //variables
        bool _isGenerable;
        bool _isChangable;
        Point _currentCellIndex;
        Random _random;
        int _fallCounter;
        int _moveRoghtAndLeftCounter;
        int _currentGameTime;
        
        //constructor
        public ElectrisPhysics()
        {
            _isGenerable = true;
            _isChangable = true;
            _currentCellIndex = new Point(15, 2);
            _random = new Random();
            _currentGameTime = 0;            
            _fallCounter = 0;
            _moveRoghtAndLeftCounter = 0;
        }

        //Update function
        public void Update(KeyboardState keyboard)
        {
            #region update_falling_delay
            if (!ELECTRISSHAREDDATA.Instance._isSpacePressed)
            {
                int emotion = MINIGAMESDATA.Instance._maxEmotionValue - MINIGAMESDATA.Instance._frustration;
                if (emotion > 0)
                    ELECTRISSHAREDDATA.Instance._currentFallingDelay = ELECTRISSHAREDDATA.Instance._fallingDelay * 1 / emotion;
                else
                {
                    ELECTRISSHAREDDATA.Instance._currentFallingDelay = ELECTRISSHAREDDATA.Instance._fallingDelay;
                }
            }
            #endregion

            #region Space_Hit
            //check the keyboard
            if (keyboard.IsKeyDown(Keys.Space) && !ELECTRISSHAREDDATA.Instance._isSpacePressed)
            {
                ELECTRISSHAREDDATA.Instance._isSpacePressed = true;
                _isChangable = false;
                //ELECTRISSHAREDDATA.Instance._fallingDelay = (int)ELECTRISSHAREDDATA.Instance._fallingDelay;
                ELECTRISSHAREDDATA.Instance._currentFallingDelay = ELECTRISSHAREDDATA.Instance._currentFallingDelay / 4;
            }
            #endregion

            #region Quit_Hit
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                //check for the game result
                ELECTRISSHAREDDATA.Instance._currentGameResult = false;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
                //log
                TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                ELECTRISSHAREDDATA.Instance._electrisLogStr += timeStamp.TotalSeconds.ToString() + "\t";
                ELECTRISSHAREDDATA.Instance._electrisLogStr += ELECTRISSHAREDDATA.Instance._currentGameResult ? "1" : "0";
                ELECTRISSHAREDDATA.Instance._electrisLogStr += "\t";
            }
            #endregion

            //generate a new item
            //if it is generable and the first cell is empty now!
            Generate();

            //update counters
            _fallCounter++;
            _moveRoghtAndLeftCounter++;

            #region Right_Lerft_Keys_Hit
            if (keyboard.IsKeyDown(Keys.Right))
            {
                if (_moveRoghtAndLeftCounter > ELECTRISSHAREDDATA.Instance._keyboardDelay)
                {
                    MoveToRight();
                    _moveRoghtAndLeftCounter = 0;
                }    
            }
            else if (keyboard.IsKeyDown(Keys.Left))
            {
                if (_moveRoghtAndLeftCounter > ELECTRISSHAREDDATA.Instance._keyboardDelay)
                {
                    MoveToLeft();
                    _moveRoghtAndLeftCounter = 0;
                }
            }
            #endregion

            #region Falling
            if (_fallCounter > ELECTRISSHAREDDATA.Instance._currentFallingDelay)
            {
                //falling
                Fall();

                //change the falling cell
                ChangeCell(ELECTRISSHAREDDATA.Instance._cells[_currentCellIndex.X, _currentCellIndex.Y]);

                //reset the falling counter
                 _fallCounter = 0;
            }
            #endregion

            #region prune
            //chack for the pattern and prune if it's necessary
            if (_isGenerable)
                CheckPatternAndPrune();
            #endregion

            #region check_the_game_status
            if (IsGameFinished())
            {
                ELECTRISSHAREDDATA.Instance._currentGameResult = true;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
                //log
                TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                ELECTRISSHAREDDATA.Instance._electrisLogStr += timeStamp.TotalSeconds.ToString() + "\t";
                ELECTRISSHAREDDATA.Instance._electrisLogStr += ELECTRISSHAREDDATA.Instance._currentGameResult ? "1" : "0";
                ELECTRISSHAREDDATA.Instance._electrisLogStr += "\t";
            }
            #endregion

        }

        //move the falling component to right
        private void MoveToRight()
        {
            ELECTRISSHAREDDATA obj = ELECTRISSHAREDDATA.Instance;
            //if there is more space on the right side
            if (_currentCellIndex.Y < ELECTRISSHAREDDATA.Instance._numberOfColumns - 1 && _isChangable)
            {
                if (obj._cells[_currentCellIndex.X, _currentCellIndex.Y + 1]._isEmpty)
                {
                    //switch cells
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y + 1]._isEmpty = obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._isEmpty;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y + 1]._texture = obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._texture;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y + 1]._isItFixed = false;
                    //clean the previous cell
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._isEmpty = true;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._texture = ELECTRISSHAREDDATA.TextureEnum.blank_TAG;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._isItFixed = false;
                    //change the current index
                    _currentCellIndex.Y++;
                }
            }
        }

        //move the falling component to the left
        private void MoveToLeft()
        {
            ELECTRISSHAREDDATA obj = ELECTRISSHAREDDATA.Instance;
            //if there is more space on the right side
            if (_currentCellIndex.Y > 0 && _isChangable)
            {
                if (obj._cells[_currentCellIndex.X, _currentCellIndex.Y - 1]._isEmpty)
                {
                    //switch cells
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y - 1]._isEmpty = obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._isEmpty;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y - 1]._texture = obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._texture;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y - 1]._isItFixed = false;
                    //clean the previous cell
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._isEmpty = true;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._texture = ELECTRISSHAREDDATA.TextureEnum.blank_TAG;
                    obj._cells[_currentCellIndex.X, _currentCellIndex.Y]._isItFixed = false;
                    //change the current index
                    _currentCellIndex.Y--;
                }
            }
        }

        //generate the different componenets by GenerateComponent() function
        private void Generate()
        {
            if (_isGenerable)
            {
                //reset the space status btn
                ELECTRISSHAREDDATA.Instance._isSpacePressed = false;
                //initiate it here
                _currentCellIndex.X = 15;
                _currentCellIndex.Y = 2;
                //put it in the cell
                ELECTRISSHAREDDATA.Instance._cells[_currentCellIndex.X, _currentCellIndex.Y]._texture = (ELECTRISSHAREDDATA.TextureEnum)_random.Next(1, 6);
                ELECTRISSHAREDDATA.Instance._cells[_currentCellIndex.X, _currentCellIndex.Y]._isEmpty = false;
                ELECTRISSHAREDDATA.Instance._cells[_currentCellIndex.X, _currentCellIndex.Y]._isItFixed = false;
                //set the falg to false to prevent generating more items ot once!
                _isGenerable = false;
                _isChangable = true;
            }
        }

        //falling function
        private void Fall()
        {
            //it is generable unless there is at least one move
            _isGenerable = true;

            //travers all the cells
            //if the cell is full and the underneath cell is empty move it down
            ELECTRISSHAREDDATA obj = ELECTRISSHAREDDATA.Instance;
            for (int i = 0; i < ELECTRISSHAREDDATA.Instance._numberOfRows; i++)
            {
                for (int j = 0; j < ELECTRISSHAREDDATA.Instance._numberOfColumns; j++)
                {
                    if (!obj._cells[i, j]._isEmpty && i > 0)
                    {
                        if (obj._cells[i-1, j]._isEmpty)
                        {
                            //switch cells
                            obj._cells[i-1, j]._isEmpty = obj._cells[i, j]._isEmpty;
                            obj._cells[i-1, j]._texture = obj._cells[i, j]._texture;
                            //clean the previous cell
                            obj._cells[i, j]._isEmpty = true;
                            obj._cells[i, j]._texture = ELECTRISSHAREDDATA.TextureEnum.blank_TAG;
                            //check to see if the item is fixed or not
                            if (!obj._cells[i, j]._isItFixed)
                            {
                                //set the current cell's flag
                                _currentCellIndex.X = i - 1;
                                _currentCellIndex.Y = j;
                                //set the cell's flag
                                obj._cells[i, j]._isItFixed = true;
                                obj._cells[i - 1, j]._isItFixed = false;
                                
                            }
                            //set the isGenerable flag to false
                            _isGenerable = false;
                        }   
                    }
                }
            }
        }

        //This function changes the cmponents randomly while it's falling down
        private void ChangeCell(ELECTRISSHAREDDATA.Cell cell)
        {
            //if the underneath cell is still empty
            if (_isChangable && cell._index.X > 0)
            {
                if (ELECTRISSHAREDDATA.Instance._cells[cell._index.X - 1, cell._index.Y]._isEmpty)
                    cell._texture = (ELECTRISSHAREDDATA.TextureEnum)_random.Next(1, 6);
            }
        }

        //This function checks the pattern and call the prune function if needed
        private void CheckPatternAndPrune()
        {
            bool passedRow;
            for (int i = 0; i < ELECTRISSHAREDDATA.Instance._numberOfRows; i++)
            {
                passedRow = true;
                for (int j = 0; j < ELECTRISSHAREDDATA.Instance._numberOfColumns; j++)
                {
                    //check for the pattern
                    if (ELECTRISSHAREDDATA.Instance._cells[i, j]._texture != (ELECTRISSHAREDDATA.TextureEnum)ELECTRISSHAREDDATA.Instance._pattern[j])
                        passedRow = false;
                    //check for the same components
                    if (j < ELECTRISSHAREDDATA.Instance._numberOfColumns - 1)
                    {
                        if (ELECTRISSHAREDDATA.Instance._cells[i, j]._texture == ELECTRISSHAREDDATA.Instance._cells[i, j + 1]._texture)
                            PruneComponents(i, j);
                    }
                }
                if (passedRow)
                {
                    PruneRow(i);
                }
            }
        }

        //delete a row
        private void PruneRow(int row)
        {
            ELECTRISSHAREDDATA obj = ELECTRISSHAREDDATA.Instance;
            for (int i = row; i < ELECTRISSHAREDDATA.Instance._numberOfRows - 1; i++)
            {
                for (int j = 0; j < ELECTRISSHAREDDATA.Instance._numberOfColumns; j++)
                {
                    //switch cells
                    obj._cells[i, j]._isEmpty = obj._cells[i+1, j]._isEmpty;
                    obj._cells[i, j]._texture = obj._cells[i+1, j]._texture;
                    obj._cells[i, j]._isEmpty = obj._cells[i + 1, j]._isEmpty;
                    obj._cells[i, j]._isItFixed = obj._cells[i + 1, j]._isItFixed;
                }
            }
            ELECTRISSHAREDDATA.Instance._numberOfFinishedRows++;
        }
        
        //delete similar components
        private void PruneComponents(int row, int column)
        {
            ELECTRISSHAREDDATA.Instance._cells[row, column]._texture = ELECTRISSHAREDDATA.TextureEnum.blank_TAG;
            ELECTRISSHAREDDATA.Instance._cells[row, column]._isEmpty = true;
            ELECTRISSHAREDDATA.Instance._cells[row, column]._isItFixed = true;
            ELECTRISSHAREDDATA.Instance._cells[row, column + 1]._texture = ELECTRISSHAREDDATA.TextureEnum.blank_TAG;
            ELECTRISSHAREDDATA.Instance._cells[row, column + 1]._isEmpty = true;
            ELECTRISSHAREDDATA.Instance._cells[row, column + 1]._isItFixed = true;
        }

        //check the game status
        private bool IsGameFinished()
        {
            if (ELECTRISSHAREDDATA.Instance._currentNumberOfRows == ELECTRISSHAREDDATA.Instance._numberOfFinishedRows)
                return true;
            else
                return false;
        }

    }


    
}
