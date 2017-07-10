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
        CCLabel label;
        CCSprite paddleSprite;
        Box box;

        public IntroLayer() : base(CCColor4B.Black)
        {
            label = new CCLabel("Hello Hatehim10", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            AddChild(label);

            paddleSprite = new CCSprite("paddle");
            paddleSprite.PositionX = 100;
            paddleSprite.PositionY = 100;
            AddChild(paddleSprite);

            box = new Box();
            box.PositionX = 640;
            box.PositionY = 320;
            box.Width = 32;
            box.Height = 32;
            AddChild(box);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            label.Position = bounds.Center;

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
                paddleSprite.RunAction(new CCMoveTo(.1f, new CCPoint(touches[0].Location.X, paddleSprite.PositionY)));
            }
        }

        void OnTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                paddleSprite.PositionX = touches[0].Location.X;
            }
        }

        float ballXVelocity;
        float ballYVelocity;

        const float gravity = 140;

        int score = 0;

        public void RunGameLogic(float frameTimeInSeconds)
        {
            box.Draw();
            ballYVelocity += frameTimeInSeconds * -gravity;

            box.PositionX += ballXVelocity * frameTimeInSeconds;
            box.PositionY += ballYVelocity * frameTimeInSeconds;

            //Check if overlap
            bool doesBallOverlapPaddle = box.BoundingBoxTransformedToParent.IntersectsRect(paddleSprite.BoundingBoxTransformedToParent);
            bool isMovingDownward = ballYVelocity < 0;
            if (doesBallOverlapPaddle && isMovingDownward)
            {
                ballYVelocity *= -1;

                const float minXVelocity = -300;
                const float maxXVelocity = 300;
                ballXVelocity = CCRandom.GetRandomFloat(minXVelocity, maxXVelocity);

                score++;
                label.Text = string.Format("Score: {0}", score);
            }

            float ballRight = box.BoundingBoxTransformedToParent.MaxX;
            float ballLeft = box.BoundingBoxTransformedToParent.MinX;

            float screenRight = VisibleBoundsWorldspace.MaxX;
            float screenLeft = VisibleBoundsWorldspace.MinX;

            bool shouldReflectXVelocity = (ballRight > screenRight && ballXVelocity > 0) || (ballLeft < screenLeft && ballXVelocity < 0);
            if (shouldReflectXVelocity)
            {
                ballXVelocity *= -1;
            }

            if (box.PositionY < VisibleBoundsWorldspace.MinY)
            {
                box.PositionX = 640;
                box.PositionY = 320;

                ballXVelocity = 0;
                ballYVelocity = 0;
                score = 0;
                label.Text = "Score: 0";
            }
        }
    }
}

