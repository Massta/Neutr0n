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
        public const int MAX_ENEMIES = 15;

        Box Player;
        MoveDirection PlayerMoveDirection;
        int PlayerSpeed;

        List<AIBox> Enemies;

        public IntroLayer() : base(CCColor4B.Black)
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            Player = new Box
            {
                PositionX = VisibleBoundsWorldspace.MidX - 50,
                PositionY = VisibleBoundsWorldspace.MidY - 50,
                Width = 100,
                Height = 100,
                BoxColor = CCColor4B.Blue
            };
            AddChild(Player);
            Player.Draw();

            PlayerMoveDirection = MoveDirection.MidMid;
            PlayerSpeed = 5;

            Enemies = new List<AIBox>();

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesBegan;
            touchListener.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(touchListener, this);
            Schedule(RunGameLogic);
        }
        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            PlayerMoveDirection = MoveDirection.MidMid;
        }

        void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                #region Touchregion calculation

                float xSize = VisibleBoundsWorldspace.MidX / 3;
                float ySize = xSize;
                //Bottom row
                CCRect bottomLeft = new CCRect
                {
                    Origin = new CCPoint(0, 0),
                    Size = new CCSize(xSize, ySize)
                };
                CCRect bottomMid = new CCRect
                {
                    Origin = new CCPoint(xSize, 0),
                    Size = new CCSize(xSize, ySize)
                };
                CCRect bottomRight = new CCRect
                {
                    Origin = new CCPoint(xSize * 2, 0),
                    Size = new CCSize(xSize, ySize)
                };
                //Middle row
                CCRect midLeft = new CCRect
                {
                    Origin = new CCPoint(0, ySize),
                    Size = new CCSize(xSize, ySize)
                };
                CCRect midMid = new CCRect
                {
                    Origin = new CCPoint(xSize, ySize),
                    Size = new CCSize(xSize, ySize)
                };
                CCRect midRight = new CCRect
                {
                    Origin = new CCPoint(xSize * 2, ySize),
                    Size = new CCSize(xSize, ySize)
                };
                //Top row
                CCRect topLeft = new CCRect
                {
                    Origin = new CCPoint(0, ySize * 2),
                    Size = new CCSize(xSize, ySize)
                };
                CCRect topMid = new CCRect
                {
                    Origin = new CCPoint(xSize, ySize * 2),
                    Size = new CCSize(xSize, ySize)
                };
                CCRect topRight = new CCRect
                {
                    Origin = new CCPoint(xSize * 2, ySize * 2),
                    Size = new CCSize(xSize, ySize)
                };

                #endregion

                #region Intersection calculation

                //Bottom Row
                if (bottomLeft.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.BottomLeft;
                }
                if (bottomMid.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.BottomMid;
                }
                if (bottomRight.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.BottomRight;
                }

                //Middle Row
                if (midLeft.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.MidLeft;
                }
                if (midRight.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.MidRight;
                }

                //Right Row
                if (topLeft.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.TopLeft;
                }
                if (topMid.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.TopMid;
                }
                if (topRight.ContainsPoint(touches[0].Location))
                {
                    PlayerMoveDirection = MoveDirection.TopRight;
                }

                #endregion
            }
        }

        public void RunGameLogic(float frameTimeInSeconds)
        {
            #region Player movement

            switch (PlayerMoveDirection)
            {
                case MoveDirection.BottomLeft:
                    Player.PositionX -= PlayerSpeed;
                    Player.PositionY -= PlayerSpeed;
                    break;
                case MoveDirection.BottomMid:
                    Player.PositionY -= PlayerSpeed;
                    break;
                case MoveDirection.BottomRight:
                    Player.PositionX += PlayerSpeed;
                    Player.PositionY -= PlayerSpeed;
                    break;
                case MoveDirection.MidLeft:
                    Player.PositionX -= PlayerSpeed;
                    break;
                case MoveDirection.MidMid:
                    Player.PositionX -= 0;
                    Player.PositionY -= 0;
                    break;
                case MoveDirection.MidRight:
                    Player.PositionX += PlayerSpeed;
                    break;
                case MoveDirection.TopLeft:
                    Player.PositionX -= PlayerSpeed;
                    Player.PositionY += PlayerSpeed;
                    break;
                case MoveDirection.TopMid:
                    Player.PositionY += PlayerSpeed;
                    break;
                case MoveDirection.TopRight:
                    Player.PositionX += PlayerSpeed;
                    Player.PositionY += PlayerSpeed;
                    break;
            }

            #endregion

            if (CCRandom.GetRandomInt(0, 100) < (100 - (Enemies.Count * 10)))
            {
                int newCount = CCRandom.GetRandomInt(Math.Max(1, Player.Counter - 100), Player.Counter + 100);
                int direction = CCRandom.GetRandomInt(0, 3);
                float newVelocityX = 0;
                float newVelocityY = 0;
                float newPositionX = 0;
                float newPositionY = 0;
                switch (direction)
                {
                    case 0:
                        //left to right
                        newVelocityX = CCRandom.GetRandomInt(3, 7);
                        newVelocityY = 0;
                        newPositionX = VisibleBoundsWorldspace.MinX - 80;
                        newPositionY = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinY, VisibleBoundsWorldspace.MaxY);
                        break;
                    case 1:
                        //top to bottom
                        newVelocityX = 0;
                        newVelocityY = CCRandom.GetRandomInt(3, 7);
                        newPositionX = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinX, VisibleBoundsWorldspace.MaxX);
                        newPositionY = VisibleBoundsWorldspace.MinY - 80;
                        break;
                    case 2:
                        //right to left
                        newVelocityX = -CCRandom.GetRandomInt(3, 7);
                        newVelocityY = 0;
                        newPositionX = VisibleBoundsWorldspace.MaxX + 80;
                        newPositionY = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinY, VisibleBoundsWorldspace.MaxY);
                        break;
                    case 3:
                        //bottom to top
                        newVelocityX = 0;
                        newVelocityY = -CCRandom.GetRandomInt(3, 7);
                        newPositionX = CCRandom.GetRandomFloat(VisibleBoundsWorldspace.MinX, VisibleBoundsWorldspace.MaxX);
                        newPositionY = VisibleBoundsWorldspace.MaxY + 80;
                        break;
                }
                AIBox newEnemy = new AIBox
                {
                    Counter = newCount,
                    Width = 80,
                    Height = 80,
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
            foreach (AIBox enemy in Enemies)
            {
                //Move enemy
                enemy.PositionX = enemy.PositionX += enemy.VelocityX;
                enemy.PositionY = enemy.PositionY += enemy.VelocityY;
                //If enemy hits box, start collision event
            }

            //If enemy is out of bounds, remove it
            var toRemove = Enemies.Where(v => v.PositionX > VisibleBoundsWorldspace.MaxX + 100
                                            || v.PositionX < VisibleBoundsWorldspace.MinX - 100
                                            || v.PositionY > VisibleBoundsWorldspace.MaxY + 100
                                            || v.PositionY < VisibleBoundsWorldspace.MinY - 100);
            foreach (var removeEnemy in toRemove)
            {
                RemoveChild(removeEnemy);
            }
            Enemies.RemoveAll(v => v.PositionX > VisibleBoundsWorldspace.MaxX + 100
                                            || v.PositionX < VisibleBoundsWorldspace.MinX - 100
                                            || v.PositionY > VisibleBoundsWorldspace.MaxY + 100
                                            || v.PositionY < VisibleBoundsWorldspace.MinY - 100);
        }
    }
}

