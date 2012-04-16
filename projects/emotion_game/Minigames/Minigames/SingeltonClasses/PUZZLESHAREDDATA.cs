﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinigamesLibrary;

namespace Minigames.SingeltonClasses
{
    class PUZZLESHAREDDATA
    {
        //static instance to be shared
        private static PUZZLESHAREDDATA instance;

        public enum PuzzleTypeEnum
        {
            nonadaptivePuzzle3Disks_TAG = 1, nonadaptivePuzzle4Disks_TAG, nonadaptivePuzzle5Disks_TAG, nonadaptivePuzzle6Disks_TAG, nonadaptivePuzzle7Disks_TAG, nonadaptivePuzzle8Disks_TAG,
            adaptivePuzzle3Disks_TAG, adaptivePuzzle4Disks_TAG, adaptivePuzzle5Disks_TAG, adaptivePuzzle6Disks_TAG, adaptivePuzzle7Disks_TAG, adaptivePuzzle8Disks_TAG
        };

        //puzzle fields
        public List<int> _puzzleTypes;
        public List<PuzzleDisk> _puzzleDisksDegrees;

        public double[] _currentDegrees;
        public int _currentNumberOfDisks;
        public bool[] _isActive;

        public int _defaultSpeed;
        public double _currentSpeed;
        public int _degreeOfFreedom;
        public bool _currentGameResult;
        public PuzzleTypeEnum _currentPuzzleType;
        public string _puzzleLogStr;

        //constructor
        private PUZZLESHAREDDATA()
        {
            _currentGameResult = false;
            _degreeOfFreedom = 5;
            _puzzleLogStr = "";
        }

        //public get function
        public static PUZZLESHAREDDATA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PUZZLESHAREDDATA();
                }
                return instance;
            }
        }

        public void Reset()
        {
            //reset the game status
            _currentGameResult = false;
            _currentSpeed = _defaultSpeed;

            if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.nonadaptivePuzzle3Disks_TAG)
            {
                _currentNumberOfDisks = 3;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[0]._degrees[i];
                }
            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.nonadaptivePuzzle4Disks_TAG)
            {
                _currentNumberOfDisks = 4;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[1]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.nonadaptivePuzzle5Disks_TAG)
            {
                _currentNumberOfDisks = 5;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[2]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.nonadaptivePuzzle6Disks_TAG)
            {
                _currentNumberOfDisks = 6;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[3]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.nonadaptivePuzzle7Disks_TAG)
            {
                _currentNumberOfDisks = 7;
                _currentDegrees = new double[_currentNumberOfDisks];
    
                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[4]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.nonadaptivePuzzle8Disks_TAG)
            {
                _currentNumberOfDisks = 8;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[5]._degrees[i];
                }

            }
            if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.adaptivePuzzle3Disks_TAG)
            {
                _currentNumberOfDisks = 3;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[0]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.adaptivePuzzle4Disks_TAG)
            {
                _currentNumberOfDisks = 4;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[1]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.adaptivePuzzle5Disks_TAG)
            {
                _currentNumberOfDisks = 5;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[2]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.adaptivePuzzle6Disks_TAG)
            {
                _currentNumberOfDisks = 6;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[3]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.adaptivePuzzle7Disks_TAG)
            {
                _currentNumberOfDisks = 7;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[4]._degrees[i];
                }

            }
            else if (PUZZLESHAREDDATA.Instance._currentPuzzleType == PuzzleTypeEnum.adaptivePuzzle8Disks_TAG)
            {
                _currentNumberOfDisks = 8;
                _currentDegrees = new double[_currentNumberOfDisks];

                _isActive = new bool[_currentNumberOfDisks];
                for (int i = 0; i < _currentNumberOfDisks; i++)
                {
                    _isActive[i] = false;
                    _currentDegrees[i] = _puzzleDisksDegrees[5]._degrees[i];
                }

            }

            //log
            _puzzleLogStr += "Puzzle" + _currentNumberOfDisks.ToString() + "\t";
            TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            _puzzleLogStr += timeStamp.TotalSeconds.ToString() + "\t";
        }
    }
}
