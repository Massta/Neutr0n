using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using Neutr0n.Shared.Models;

namespace Neutr0n.Shared
{
    public class IntroLayer : CCLayerColor
    {

        // Define a label variable
        CCDrawNode player;

        public IntroLayer() : base(CCColor4B.Black)
        {
            player = new CCDrawNode { PositionX = VisibleBoundsWorldspace.MidX-64, PositionY = VisibleBoundsWorldspace.MidY-64 };
            AddChild(player);
            player.DrawRect(new CCRect(PositionX, PositionY, 128, 128), CCColor4B.Blue);
            player.DrawString((int)(PositionX + 10), (int)(PositionY + 10), "{0}", 10);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

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
        }
    }
}

