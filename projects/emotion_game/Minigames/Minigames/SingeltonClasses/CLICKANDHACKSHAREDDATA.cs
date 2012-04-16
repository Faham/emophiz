using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MinigamesLibrary;

namespace Minigames.SingeltonClasses
{
    class CLICKANDHACKSHAREDDATA
    {
        //static instance to be shared
        private static CLICKANDHACKSHAREDDATA instance;

        public enum ClickAndHackTypeEnum
        {
            nonadaptiveClickAndHack10_TAG = 13, nonadaptiveClickAndHack20_TAG, nonadaptiveClickAndHack30_TAG, nonadaptiveClickAndHack40_TAG, nonadaptiveClickAndHack50_TAG, nonadaptiveClickAndHack60_TAG,
            adaptiveClickAndHack10_TAG, adaptiveClickAndHack20_TAG, adaptiveClickAndHack30_TAG, adaptiveClickAndHack40_TAG, adaptiveClickAndHack50_TAG, adaptiveClickAndHack60_TAG
        };

        public int _currentNumberOfNodes;
        public ClickAndHackTypeEnum _currentClickAndHackType;
        public List<Node> _nodePositions;
        public List<Vector2> _currentNodePositions;
        public int _currentNodeIndex;
        public Vector2 _smallNodeSize;
        public Vector2 _largeNodeSize;
        public Vector2 _hackBtnSize;
        public bool _isHackBtnClicked;

        public bool _currentGameResult;

        //constructor
        private CLICKANDHACKSHAREDDATA()
        {
            _currentGameResult = false;

        }

        //public get function
        public static CLICKANDHACKSHAREDDATA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CLICKANDHACKSHAREDDATA();
                }
                return instance;
            }
        }
        
        //
        //Reset function
        //
        public void Reset()
        {
            //reset the current node number
            _currentNodeIndex = 0;

            //reset the game status
            _currentGameResult = false;

            //set the number of nodes
            if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.adaptiveClickAndHack10_TAG)
            {
                _currentNumberOfNodes = 10;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[0]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.adaptiveClickAndHack20_TAG)
            {
                _currentNumberOfNodes = 20;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[1]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.adaptiveClickAndHack30_TAG)
            {
                _currentNumberOfNodes = 30;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[2]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.adaptiveClickAndHack40_TAG)
            {
                _currentNumberOfNodes = 40;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[3]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.adaptiveClickAndHack50_TAG)
            {
                _currentNumberOfNodes = 50;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[4]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.adaptiveClickAndHack60_TAG)
            {
                _currentNumberOfNodes = 60;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[5]._position;
            }
            //
            if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.nonadaptiveClickAndHack10_TAG)
            {
                _currentNumberOfNodes = 10;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[0]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.nonadaptiveClickAndHack20_TAG)
            {
                _currentNumberOfNodes = 20;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[1]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.nonadaptiveClickAndHack30_TAG)
            {
                _currentNumberOfNodes = 30;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[2]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.nonadaptiveClickAndHack40_TAG)
            {
                _currentNumberOfNodes = 40;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[3]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.nonadaptiveClickAndHack50_TAG)
            {
                _currentNumberOfNodes = 50;
                _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[4]._position;
            }
            else if (CLICKANDHACKSHAREDDATA.Instance._currentClickAndHackType == ClickAndHackTypeEnum.nonadaptiveClickAndHack60_TAG)
            {
                _currentNumberOfNodes = 60;
               _currentNodePositions = CLICKANDHACKSHAREDDATA.instance._nodePositions[5]._position;
            }

            //set general variables and flags
            _isHackBtnClicked = false;
        }
    }
}
