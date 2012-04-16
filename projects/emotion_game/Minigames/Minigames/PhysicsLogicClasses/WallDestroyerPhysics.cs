﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Minigames.SingeltonClasses;
using Microsoft.Xna.Framework;

namespace Minigames.PhysicsLogicClasses
{
    class WallDestroyerPhysics
    {
        //variables
        //enum variable to define the collision types
        enum _CollisionTypesEnum {null_TAG =0, brick_TAG, rightSide_TAG, leftSide_TAG, bottom_TAG, up_TAG, board_TAG,
        ballAndRightBrick_TAG, ballAndLeftBrick_TAG, ballAndUpBrick_TAG, ballAndBottomBrick_TAG};
        //struct variable to define the collision details between brick and the ball
        struct _BallAndBrickCollision
        {
            public int brickIndex;
            public _CollisionTypesEnum collisionType;
        }
        //temporary position of ball to prevent collision s
        Vector2 _tempPosition;

        //constructor
        public WallDestroyerPhysics()
        {
            _tempPosition = new Vector2(WALLDESTROYERSHAREDDATA.Instance._boardPosition.X, WALLDESTROYERSHAREDDATA.Instance._boardPosition.Y); 
        }

        //update function
        public void Update(KeyboardState keyboard)
        {
            //detect the key hits

            #region Quit_Hit
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                //check for the game result
                WALLDESTROYERSHAREDDATA.Instance._currentGameResult = false;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
                //log
                TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += timeStamp.TotalSeconds.ToString() + "\t";
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += WALLDESTROYERSHAREDDATA.Instance._currentGameResult ? "1" : "0";
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += "\t";
            }
            #endregion

            #region Right_Key_Hit
            if (keyboard.IsKeyDown(Keys.Right))
            {
                _tempPosition.X = WALLDESTROYERSHAREDDATA.Instance._boardPosition.X + WALLDESTROYERSHAREDDATA.Instance._currentBoardSpeed;
                if (!BoardCollisionDetection(_tempPosition, WALLDESTROYERSHAREDDATA.Instance._boardSize))
                    WALLDESTROYERSHAREDDATA.Instance._boardPosition.X += WALLDESTROYERSHAREDDATA.Instance._currentBoardSpeed;
                if (!WALLDESTROYERSHAREDDATA.Instance._isShoot)
                    WALLDESTROYERSHAREDDATA.Instance._ballPosition.X = WALLDESTROYERSHAREDDATA.Instance._boardPosition.X;
            }
            #endregion

            #region Left_Key_Hit
            if (keyboard.IsKeyDown(Keys.Left))
            {
                _tempPosition.X = WALLDESTROYERSHAREDDATA.Instance._boardPosition.X - WALLDESTROYERSHAREDDATA.Instance._currentBoardSpeed;
                if (!BoardCollisionDetection(_tempPosition, WALLDESTROYERSHAREDDATA.Instance._boardSize))
                    WALLDESTROYERSHAREDDATA.Instance._boardPosition.X -= WALLDESTROYERSHAREDDATA.Instance._currentBoardSpeed;
                if (!WALLDESTROYERSHAREDDATA.Instance._isShoot)
                    WALLDESTROYERSHAREDDATA.Instance._ballPosition.X = WALLDESTROYERSHAREDDATA.Instance._boardPosition.X;
            }
            #endregion

            #region Up_Key_Hit
            if (keyboard.IsKeyDown(Keys.Up) && !WALLDESTROYERSHAREDDATA.Instance._isShoot)
            {
                if (WALLDESTROYERSHAREDDATA.Instance._compassHnadleAngleInt + WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleSpeed < 0)
                {
                    WALLDESTROYERSHAREDDATA.Instance._compassHnadleAngleInt += WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleSpeed;
                    WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleRadian = MathHelper.ToRadians(WALLDESTROYERSHAREDDATA.Instance._compassHnadleAngleInt);
                }

            }
            #endregion

            #region Down_Key_Hit
            if (keyboard.IsKeyDown(Keys.Down) && !WALLDESTROYERSHAREDDATA.Instance._isShoot)
            {
                if (WALLDESTROYERSHAREDDATA.Instance._compassHnadleAngleInt - WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleSpeed > -180)
                {
                    WALLDESTROYERSHAREDDATA.Instance._compassHnadleAngleInt -= WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleSpeed;
                    WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleRadian = MathHelper.ToRadians(WALLDESTROYERSHAREDDATA.Instance._compassHnadleAngleInt);
                }

            }
            #endregion

            #region Space_Hey_Hit
            if (keyboard.IsKeyDown(Keys.Space) && !WALLDESTROYERSHAREDDATA.Instance._isShoot)
            {
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.X = (float)(WALLDESTROYERSHAREDDATA.Instance._ballInitialSpeed * Math.Cos(WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleRadian));
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.Y = (float)(WALLDESTROYERSHAREDDATA.Instance._ballInitialSpeed * Math.Sin(WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleRadian));
                WALLDESTROYERSHAREDDATA.Instance._isShoot = true;
            }
            #endregion

            #region move_ball
            if (WALLDESTROYERSHAREDDATA.Instance._isShoot)
            {
                //move the ball

                //get the next ball position
                Vector2 ballTemporaryPosition = new Vector2((float)(WALLDESTROYERSHAREDDATA.Instance._ballPosition.X + WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.X),
                    (float)(WALLDESTROYERSHAREDDATA.Instance._ballPosition.Y + WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.Y));
                ChangeDirectionAndSpeed(ballTemporaryPosition, WALLDESTROYERSHAREDDATA.Instance._ballSize);
            }
            #endregion

            #region check_game_status
            if (WALLDESTROYERSHAREDDATA.Instance._numberOfBalls > WALLDESTROYERSHAREDDATA.Instance._numberOfAvailableBalls)
            {//game over!
                WALLDESTROYERSHAREDDATA.Instance._currentGameResult = false;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
                //log
                TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += timeStamp.TotalSeconds.ToString() + "\t";
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += WALLDESTROYERSHAREDDATA.Instance._currentGameResult ? "1" : "0";
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += "\t";
            }
            if (WALLDESTROYERSHAREDDATA.Instance._numberOfHitBricks == WALLDESTROYERSHAREDDATA.Instance._currentNumberOfBricks)
            {
                WALLDESTROYERSHAREDDATA.Instance._currentGameResult = true;
                //set the minigame status
                MINIGAMESDATA.Instance._isMinigameRunning = false;
                //change the interface
                MINIGAMESDATA.Instance._currentMiniGame = MINIGAMESDATA.MinigamesEnum.minigamePortal_TAG;
                //log
                TimeSpan timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += timeStamp.TotalSeconds.ToString() + "\t";
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += WALLDESTROYERSHAREDDATA.Instance._currentGameResult ? "1" : "0";
                WALLDESTROYERSHAREDDATA.Instance._wallDestroyerLogStr += "\t";
            }
            //check for bricks
            #endregion
        }

        //check the bricks collisions
        //return-1 as index for no collision and return positive number for any other collisions
        
        private _BallAndBrickCollision BrickCollisionDetection(Vector2 position, int size)
        {
            _BallAndBrickCollision result = new _BallAndBrickCollision();
            result.brickIndex = -1;
            int counter = 0;
            //For the sake of optimization: checks to see whether the ball is in the brick's area or not
            if (position.Y <= WALLDESTROYERSHAREDDATA.Instance._brickLst[WALLDESTROYERSHAREDDATA.Instance._currentNumberOfBricks-1]._position.Y + WALLDESTROYERSHAREDDATA.Instance._brickSize.Y)
            {
                //get the center of the circle
                Point center = new Point((int)(position.X + size / 2), (int)(position.Y + size / 2));
                //detect collision of ball with bottom of bricks
                for (int i = 0; i < WALLDESTROYERSHAREDDATA.Instance._currentNumberOfBricks; i++)
                {
                    //detect up side collision
                    Rectangle targetRect = new Rectangle((int)WALLDESTROYERSHAREDDATA.Instance._brickLst[i]._position.X,
                        (int)WALLDESTROYERSHAREDDATA.Instance._brickLst[i]._position.Y,
                        (int)WALLDESTROYERSHAREDDATA.Instance._brickSize.X,
                        (int)WALLDESTROYERSHAREDDATA.Instance._brickSize.Y);
                    
                    if (targetRect.Intersects(new Rectangle((int)position.X, (int)position.Y, size, size))
                        && !WALLDESTROYERSHAREDDATA.Instance._brickLst[i]._isHit)
                    {
                        Rectangle collisionRect = Rectangle.Intersect(targetRect, new Rectangle((int)position.X, (int)position.Y, size, size));
                        if (collisionRect.Width >= collisionRect.Height)
                        {
                            if (center.Y > targetRect.Y + targetRect.Height / 2)//up side brick
                            {
                                result.brickIndex = counter;
                                result.collisionType = _CollisionTypesEnum.ballAndUpBrick_TAG;
                                break;
                            }
                            else
                            {
                                result.brickIndex = counter;
                                result.collisionType = _CollisionTypesEnum.ballAndBottomBrick_TAG;
                                break;
                            }
                        }
                        else if (collisionRect.Width < collisionRect.Height)
                        {
                            if (center.X < targetRect.X + targetRect.Width / 2)//right side brick
                            {
                                result.brickIndex = counter;
                                result.collisionType = _CollisionTypesEnum.ballAndRightBrick_TAG;
                                break;
                            }
                            else
                            {
                                result.brickIndex = counter;
                                result.collisionType = _CollisionTypesEnum.ballAndLeftBrick_TAG;
                                break;
                            }
                        }                        
                    }    
                    counter++;
                }
            }
            return result;
        }
        
        //board collision Detection function
        private bool BoardCollisionDetection(Vector2 position, Vector2 size)
        {
            if (position.X + size.X >= OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width)
                return true;//collide to right side of the screen
            else if (position.X <= 0)
                return true;//collide to left side of the screen
            else
                return false;
        }

        //ball collision detection function
        private _CollisionTypesEnum BallCollisionDetection(Vector2 position, int size)
        {
            if (position.X + size >= OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Width)
                return _CollisionTypesEnum.rightSide_TAG;//collide to right side of the screen
            else if (position.X <= 0)
                return _CollisionTypesEnum.leftSide_TAG;//collide to left side of the screen
            else if (position.Y + size >= OBJECTS.Instance._sharedGraphicDeviceMgr.GraphicsDevice.Viewport.Height)
                return _CollisionTypesEnum.bottom_TAG;
            else if (position.Y <= 0)
                return _CollisionTypesEnum.up_TAG;
            else if (position.Y + size >= WALLDESTROYERSHAREDDATA.Instance._boardPosition.Y
                && position.X + size >= WALLDESTROYERSHAREDDATA.Instance._boardPosition.X
                && position.X <= WALLDESTROYERSHAREDDATA.Instance._boardPosition.X + WALLDESTROYERSHAREDDATA.Instance._boardSize.X)
            {
                WALLDESTROYERSHAREDDATA.Instance._isBallActive = true;
                return _CollisionTypesEnum.board_TAG;
            }
            //ball and brick collision detection
            _BallAndBrickCollision collision = BrickCollisionDetection(position, size);
            if (collision.brickIndex != -1)
            {
                //brick is hit
                if (collision.collisionType == _CollisionTypesEnum.ballAndUpBrick_TAG
                    && WALLDESTROYERSHAREDDATA.Instance._isBallActive)
                {
                    WALLDESTROYERSHAREDDATA.Instance._brickLst[collision.brickIndex]._isHit = true;
                    WALLDESTROYERSHAREDDATA.Instance._isBallActive = false;
                    WALLDESTROYERSHAREDDATA.Instance._numberOfHitBricks++;
                }
                return collision.collisionType;
            }
            else
                return _CollisionTypesEnum.null_TAG;
                
        }

        //change the direction of the ball with respect to collisions
        private void ChangeDirectionAndSpeed(Vector2 position, int size)
        {
            
            //detect the collision
            _CollisionTypesEnum collision = BallCollisionDetection(position, WALLDESTROYERSHAREDDATA.Instance._ballSize);
            //change the speed and direction
            if (collision == _CollisionTypesEnum.null_TAG)
                {
                    WALLDESTROYERSHAREDDATA.Instance._ballPosition = position;
                }
            else if (collision == _CollisionTypesEnum.rightSide_TAG || collision == _CollisionTypesEnum.leftSide_TAG)
            {
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.X *= -1;
                
            }
            else if (collision == _CollisionTypesEnum.up_TAG)
            {
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.Y *= -1;
            }
            else if (collision == _CollisionTypesEnum.board_TAG)
            {
                //set the new ball initial speed
                int emotion = MINIGAMESDATA.Instance._maxEmotionValue - MINIGAMESDATA.Instance._fun;
                if (emotion > 0)
                {
                    WALLDESTROYERSHAREDDATA.Instance._ballInitialSpeed = emotion;
                    WALLDESTROYERSHAREDDATA.Instance._currentBoardSpeed = emotion;
                }
                else
                {
                    WALLDESTROYERSHAREDDATA.Instance._ballInitialSpeed = WALLDESTROYERSHAREDDATA.Instance._defaultBallSpeed;
                    WALLDESTROYERSHAREDDATA.Instance._currentBoardSpeed = WALLDESTROYERSHAREDDATA.Instance._defaultBoardSpeed;
                }

                //set the speed
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.X = (float)Math.Abs((WALLDESTROYERSHAREDDATA.Instance._ballInitialSpeed * Math.Cos(WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleRadian))) * Math.Sign(WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.X);
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.Y = (float)(WALLDESTROYERSHAREDDATA.Instance._ballInitialSpeed * Math.Sin(WALLDESTROYERSHAREDDATA.Instance._compassHandleAngleRadian));
            }
            else if (collision == _CollisionTypesEnum.ballAndUpBrick_TAG
                || collision == _CollisionTypesEnum.ballAndBottomBrick_TAG)
            {
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.Y *= -1;
            }
            else if (collision == _CollisionTypesEnum.ballAndRightBrick_TAG
                || collision == _CollisionTypesEnum.ballAndLeftBrick_TAG)
            {
                WALLDESTROYERSHAREDDATA.Instance._currentBallSpeed.X *= -1;
            }
            else if (collision == _CollisionTypesEnum.bottom_TAG)
            {
                WALLDESTROYERSHAREDDATA.Instance.AddBall();
                WALLDESTROYERSHAREDDATA.Instance._isShoot = false;
            }
        }
    }
}
