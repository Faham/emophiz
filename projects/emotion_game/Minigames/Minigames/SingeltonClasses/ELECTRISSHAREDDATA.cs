using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Minigames.PhysicsLogicClasses;

namespace Minigames.SingeltonClasses
{
    class ELECTRISSHAREDDATA
    {
        //static instance to be shared
        private static ELECTRISSHAREDDATA instance;

        public enum ElectrisTypeEnum
        {
            nonadaptiveElectris1Row_TAG = 25, nonadaptiveElectris2Row_TAG, nonadaptiveElectris3Row_TAG,
            adaptiveElectris1Row_TAG, adaptiveElectris2Row_TAG, adaptiveElectris3Row_TAG
        };

        //this class represent the cells of the
        //electris.
        public class Cell
        {
            public Vector2 _position;
            public bool _isEmpty;
            public ELECTRISSHAREDDATA.TextureEnum _texture;
            public Point _index;
            public bool _isItFixed;
            
            //constructor
            public Cell(Vector2 position, Point index, ELECTRISSHAREDDATA.TextureEnum texture, bool empty, bool isFixed)
            {
                _position = position;
                _isEmpty = empty;
                _texture = texture;
                _index = index;
                _isItFixed = isFixed;
            }
        }


        public enum TextureEnum {blank_TAG = 0, capacitor_TAG, diod_TAG, indicator_TAG, resistor_TAG, lamp_TAG};

        public ElectrisTypeEnum _currentElectrisType;
        public bool _currentGameResult;
        public bool _isGameStarted;
        public int _currentNumberOfRows;
        public List<int> _pattern;
        
        public List<Point> _cellPositioins;
        public int _numberOfRows;
        public int _numberOfColumns;
        public int _cellWidth;
        public int _cellHeight;
        public Cell[, ] _cells;
        public int _keyboardDelay;
        public int _fallingDelay;
        public int _currentFallingDelay;
        public int _numberOfFinishedRows;
        public bool _isSpacePressed;
        public string _electrisLogStr;
        public int _logDelayCounter;
        

        //constructor
        private ELECTRISSHAREDDATA()
        {
            _currentGameResult = false;
            _electrisLogStr = "";
        }

        //public get function
        public static ELECTRISSHAREDDATA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ELECTRISSHAREDDATA();
                }
                return instance;
            }
        }

        //Reset function
        public void Reset()
        {
            //reset the log string
            _electrisLogStr = "";
            //reset the game status
            _currentGameResult = false;
            _isSpacePressed = false;
            _isGameStarted = false;

            //reset the speed
            _keyboardDelay = 10;
            _currentFallingDelay = _fallingDelay;
            _logDelayCounter = 0;

            //reset the game status
            _currentGameResult = false;
            _numberOfFinishedRows = 0;

            //set the number of rows
            if (_currentElectrisType == ElectrisTypeEnum.nonadaptiveElectris1Row_TAG)
                _currentNumberOfRows = 1;
            else if (_currentElectrisType == ElectrisTypeEnum.nonadaptiveElectris2Row_TAG)
                _currentNumberOfRows = 2;
            else if (_currentElectrisType == ElectrisTypeEnum.nonadaptiveElectris3Row_TAG)
                _currentNumberOfRows = 3;
            else if (_currentElectrisType == ElectrisTypeEnum.adaptiveElectris1Row_TAG)
                _currentNumberOfRows = 1;
            else if (_currentElectrisType == ElectrisTypeEnum.adaptiveElectris2Row_TAG)
                _currentNumberOfRows = 2;
            if (_currentElectrisType == ElectrisTypeEnum.adaptiveElectris3Row_TAG)
                _currentNumberOfRows = 3;

            //reset the cells
            _cells = new Cell[_numberOfRows, _numberOfColumns];

            for (int i = 0; i < ELECTRISSHAREDDATA.Instance._numberOfRows; i++)
            {
                for (int j = 0; j < ELECTRISSHAREDDATA.Instance._numberOfColumns; j++)
                {
                    _cells[i, j] = new Cell(new Vector2(ELECTRISSHAREDDATA.Instance._cellPositioins[j].X, ELECTRISSHAREDDATA.Instance._cellPositioins[0].Y - i * ELECTRISSHAREDDATA.Instance._cellHeight),
                        new Point(i, j), ELECTRISSHAREDDATA.TextureEnum.blank_TAG, true, true);
                }
            }

            _electrisLogStr += "Electris" + _currentNumberOfRows.ToString() + "\t";
            TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            _electrisLogStr += timeStamp.TotalSeconds.ToString() + "\t";
        }
    }
}
