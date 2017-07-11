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
        public const int MAX_ENEMIES = 20;

        Box Player;
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

            Enemies = new List<AIBox>();

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchesBegan;
            touchListener.OnTouchesMoved = OnTouchesMoved;
            AddEventListener(touchListener, this);
            Schedule(RunGameLogic);
        }

        void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                //paddleSprite.RunAction(new CCMoveTo(.1f, new CCPoint(touches[0].Location.X, paddleSprite.PositionY)));
            }
        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                //paddleSprite.PositionX = touches[0].Location.X;
            }
        }

        public void RunGameLogic(float frameTimeInSeconds)
        {
            if (CCRandom.GetRandomInt(0, 100) < (100-(Enemies.Count*10)))
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
            var toRemove = Enemies.Where(v => v.PositionX > VisibleBoundsWorldspace.MaxX+100
                                            || v.PositionX < VisibleBoundsWorldspace.MinX-100
                                            || v.PositionY > VisibleBoundsWorldspace.MaxY+100
                                            || v.PositionY < VisibleBoundsWorldspace.MinY-100);
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

