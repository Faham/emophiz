using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Minigames.SingeltonClasses
{
    class WALLDESTROYERSHAREDDATA
    {
        //
        public class Brick
        {
            //variables
            public Vector2 _position;
            public bool _isHit;

            //public constructor
            public Brick(Vector2 position, bool hitStatus)
            {
                _position = position;
                _isHit = hitStatus;
            }
        }

        //static instance to be shared
        private static WALLDESTROYERSHAREDDATA instance;

        //
        public enum WallDestroyerTypesEnum
        {
            wallDestroyer1_TAG = 31, wallDestroyer2_TAG = 32, wallDestroyer3_TAG = 33 
        };

        //variables
        public bool _currentGameResult;
        public WallDestroyerTypesEnum _currentWallDestroyerType;
        public List<Vector2> _brickPositionsLst;
        public List<Brick> _brickLst;
        public Vector2 _brickSize;
        public int _numberOfBricksInARow;
        public int _currentNumberOfBricks;
        public bool _isShoot;
        public Vector2 _ballPosition;
        public int _ballSize;
        public Vector2 _boardPosition;
        public Vector2 _boardSize;
        public int _boardHorizontalposition;
        public int _ballHorizontalPosition;
        public int _defaultBoardSpeed;
        public int _currentBoardSpeed;
        public int _defaultBallSpeed;
        public int _ballInitialSpeed;
        public Vector2 _currentBallSpeed;
        public Vector2 _compassPosition;
        public Vector2 _handlePosition;
        public float _compassHandleAngleRadian;
        public int _compassHnadleAngleInt;
        public int _compassHandleAngleSpeed;
        public bool _isBallActive;
        public int _numberOfBalls;
        public int _numberOfAvailableBalls;
        public int _numberOfHitBricks;
        public string _wallDestroyerLogStr;

        //constructor
        private WALLDESTROYERSHAREDDATA()
        {
            _brickLst = new List<Brick>();
            _wallDestroyerLogStr = "";
        }

        //public get function
        public static WALLDESTROYERSHAREDDATA Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WALLDESTROYERSHAREDDATA();
                }
                return instance;
            }
        }
        
    
        //
        //Reset function
        //
        public void Reset()
        {
            //set the result
            _currentGameResult = false;
            
            //reset flags
            _isShoot = false;
            _isBallActive = true;

            //reset the speed
            _currentBallSpeed.X = 0;
            _currentBallSpeed.Y = 0;
            _ballInitialSpeed = _defaultBallSpeed;
            _currentBoardSpeed = _defaultBoardSpeed;
            _numberOfBalls = 0;
            _numberOfHitBricks = 0;

            //reset the compass handle angle
            _compassHnadleAngleInt = -90;
            _compassHandleAngleRadian = MathHelper.ToRadians(_compassHnadleAngleInt);

            //reset the board and ball positions
            _ballPosition = new Vector2(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2, OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height - _ballHorizontalPosition);
            _boardPosition = new Vector2(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2, OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height - _boardHorizontalposition);

            //set the current number of bricks
            if (WALLDESTROYERSHAREDDATA.Instance._currentWallDestroyerType == WALLDESTROYERSHAREDDATA.WallDestroyerTypesEnum.wallDestroyer1_TAG)
            {
                _currentNumberOfBricks = _numberOfBricksInARow * 2;
                _numberOfAvailableBalls = 1;
            }
            else if (WALLDESTROYERSHAREDDATA.Instance._currentWallDestroyerType == WALLDESTROYERSHAREDDATA.WallDestroyerTypesEnum.wallDestroyer2_TAG)
            {
                _currentNumberOfBricks = _numberOfBricksInARow * 4;
                _numberOfAvailableBalls = 2;
            }
            else
            {
                _currentNumberOfBricks = _numberOfBricksInARow * 6;
                _numberOfAvailableBalls = 3;
            }

            //reset the brick list
            _brickLst.Clear();
            foreach (Vector2 pos in WALLDESTROYERSHAREDDATA.Instance._brickPositionsLst)
            {
                Brick tempBrick = new Brick(pos, false);
                _brickLst.Add(tempBrick);
            }

            //log
            _wallDestroyerLogStr += "WallDestroyer" + _currentNumberOfBricks.ToString() + "\t";
            TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            _wallDestroyerLogStr += timeStamp.TotalSeconds.ToString() + "\t";
        }

        //add ball method to add more balls
        public void AddBall()
        {
            //increase the number of balls
            _numberOfBalls++;
            //reset the board and ball positions
            _ballPosition = new Vector2(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2, OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height - _ballHorizontalPosition);
            _boardPosition = new Vector2(OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width / 2, OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height - _boardHorizontalposition);
            //reset the ball state
            _isBallActive = true;
        }
    }
}
