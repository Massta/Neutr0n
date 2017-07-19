using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using Neutr0n.Shared.Models;
using System.Linq;

namespace Neutr0n.Shared
{
    public class IntroLayer : CCLayerColor
    {
        public const int MAX_ENEMIES = 10;
        public const int MAX_PLAYER_SPEED = 7;
        public const int MAX_ENEMY_SPEED = 6;
        public const int MIN_ENEMY_SPEED = 1;

        private Box Player;
        private float PlayerHorizontalSpeed;
        private float PlayerVerticalSpeed;
        private CCPoint TouchPoint;
        private int PlayerSpeed;
        private bool PlayerIsMoving;

        List<AIBox> Enemies;

        public IntroLayer() : base(CCColor4B.Black)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            #region Object initialization

            var bounds = VisibleBoundsWorldspace;

            Player = new Box
            {
                PositionX = VisibleBoundsWorldspace.MidX - 50,
                PositionY = VisibleBoundsWorldspace.MidY - 50,
                Width = 100,
                Height = 100,
                BoxColor = CCColor4B.Blue,
                Counter = 1
            };
            AddChild(Player);
            Player.Draw();

            PlayerHorizontalSpeed = 0;
            PlayerVerticalSpeed = 0;
            PlayerSpeed = 4;
            PlayerIsMoving = false;

            Enemies = new List<AIBox>();

            TouchPoint = new CCPoint();

            #endregion

            #region Touch events

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesMoved;
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);

            #endregion

            Schedule(RunGameLogic);
        }

        void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                #region Touchregion calculation

                TouchPoint = touches[0].Location;

                #endregion
            }
        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCPoint newTouchPoint = touches[0].Location;

                CCPoint relativeTouchPoint = TouchPoint - newTouchPoint;

                #region Intersection calculation

                if (relativeTouchPoint.X != 0 && relativeTouchPoint.Y != 0)
                {
                    PlayerIsMoving = true;
                    PlayerHorizontalSpeed = (float)(-relativeTouchPoint.X / (Math.Sqrt(Math.Pow(relativeTouchPoint.X, 2) + Math.Pow(relativeTouchPoint.Y, 2))));
                    PlayerVerticalSpeed = (float)(-relativeTouchPoint.Y / (Math.Sqrt(Math.Pow(relativeTouchPoint.X, 2) + Math.Pow(relativeTouchPoint.Y, 2))));
                }

                #endregion
            }
        }
        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            PlayerHorizontalSpeed = 0;
            PlayerVerticalSpeed = 0;
            PlayerIsMoving = false;
        }

        public void RunGameLogic(float frameTimeInSeconds)
        {
            #region Player movement
            if (PlayerIsMoving)
            {
                Player.Position = TranslatePosition(Player.Position, PlayerHorizontalSpeed, PlayerVerticalSpeed, PlayerSpeed);
            }

            #endregion

            #region Enemy creation

            if (CCRandom.GetRandomInt(0, 100) < (100 - (Enemies.Count * 10)))
            {
                int newCount = CCRandom.GetRandomInt(1, Player.Counter * 10);
                int direction = CCRandom.GetRandomInt(0, 3);
                float newSize = 80;
                float newVelocityX = 0;
                float newVelocityY = 0;
                float newPositionX = 0;
                float newPositionY = 0;
                switch (direction)
                {
                    case 0:
                        //left to right
                        newVelocityX = CCRandom.GetRandomInt(MIN_ENEMY_SPEED, MAX_ENEMY_SPEED);
                        newVelocityY = 0;
                        newPositionX = VisibleBoundsWorldspace.MinX - newSize;
                        newPositionY = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinY, VisibleBoundsWorldspace.MaxY);
                        break;
                    case 1:
                        //top to bottom
                        newVelocityX = 0;
                        newVelocityY = CCRandom.GetRandomInt(MIN_ENEMY_SPEED, MAX_ENEMY_SPEED);
                        newPositionX = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinX, VisibleBoundsWorldspace.MaxX);
                        newPositionY = VisibleBoundsWorldspace.MinY - newSize;
                        break;
                    case 2:
                        //right to left
                        newVelocityX = -CCRandom.GetRandomInt(MIN_ENEMY_SPEED, MAX_ENEMY_SPEED);
                        newVelocityY = 0;
                        newPositionX = VisibleBoundsWorldspace.MaxX + newSize;
                        newPositionY = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinY, VisibleBoundsWorldspace.MaxY);
                        break;
                    case 3:
                        //bottom to top
                        newVelocityX = 0;
                        newVelocityY = -CCRandom.GetRandomInt(MIN_ENEMY_SPEED, MAX_ENEMY_SPEED);
                        newPositionX = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinX, VisibleBoundsWorldspace.MaxX);
                        newPositionY = VisibleBoundsWorldspace.MaxY + newSize;
                        break;
                }
                AIBox newEnemy = new AIBox
                {
                    Counter = newCount,
                    Width = newSize,
                    Height = newSize,
                    BoxColor = CCColor4B.Red,
                    PositionX = newPositionX,
                    PositionY = newPositionY,
                    VelocityX = newVelocityX,
                    VelocityY = newVelocityY
                };
                Enemies.Add(newEnemy);
                AddChild(newEnemy);
                newEnemy.Draw();
            }

            #endregion

            #region Enemy Movement and collision

            foreach (AIBox enemy in Enemies)
            {
                //Move enemy
                enemy.PositionX = enemy.PositionX += enemy.VelocityX;
                enemy.PositionY = enemy.PositionY += enemy.VelocityY;
                //If enemy hits box, start collision event
                if (Player.BoundingBox.IntersectsRect(enemy.BoundingBox))
                {
                    if (Player.Counter >= enemy.Counter)
                    {
                        //Eats enemy
                        Player.Counter += enemy.Counter;
                    }
                    else
                    {
                        //Dead
                        Player.Counter = 1;
                    }
                    enemy.Dead = true;
                }
            }

            #endregion

            #region Enemy removal

            //If enemy is out of bounds, remove it
            var toRemove = Enemies.Where(v => v.PositionX > VisibleBoundsWorldspace.MaxX + 100
                                            || v.PositionX < VisibleBoundsWorldspace.MinX - 100
                                            || v.PositionY > VisibleBoundsWorldspace.MaxY + 100
                                            || v.PositionY < VisibleBoundsWorldspace.MinY - 100
                                            || v.Dead);
            foreach (var removeEnemy in toRemove)
            {
                RemoveChild(removeEnemy);
            }
            Enemies.RemoveAll(v => v.PositionX > VisibleBoundsWorldspace.MaxX + 100
                                            || v.PositionX < VisibleBoundsWorldspace.MinX - 100
                                            || v.PositionY > VisibleBoundsWorldspace.MaxY + 100
                                            || v.PositionY < VisibleBoundsWorldspace.MinY - 100
                                            || v.Dead);

            #endregion
        }

        private CCPoint TranslatePosition(CCPoint startPosition, float horizontalSpeed, float verticalSpeed, int speed)
        {
            startPosition.X += horizontalSpeed * speed;
            startPosition.Y += verticalSpeed * speed;

            return startPosition;
        }
    }
}