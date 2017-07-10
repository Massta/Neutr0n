using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace Neutr0n
{
    public class IntroLayer : CCLayerColor
    {

        // Define a label variable
        CCLabel label;
        CCSprite paddleSprite, ballSprite;

        public IntroLayer() : base(CCColor4B.Black)
        {
            label = new CCLabel("Hello Hatehim10", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            AddChild(label);

            paddleSprite = new CCSprite("paddle");
            paddleSprite.PositionX = 100;
            paddleSprite.PositionY = 100;
            AddChild(paddleSprite);

            ballSprite = new CCSprite("ball");
            ballSprite.PositionX = 640;
            ballSprite.PositionY = 320;
            AddChild(ballSprite);
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
            ballYVelocity += frameTimeInSeconds * -gravity;

            ballSprite.PositionX += ballXVelocity * frameTimeInSeconds;
            ballSprite.PositionY += ballYVelocity * frameTimeInSeconds;

            //Check if overlap
            bool doesBallOverlapPaddle = ballSprite.BoundingBoxTransformedToParent.IntersectsRect(paddleSprite.BoundingBoxTransformedToParent);
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

            float ballRight = ballSprite.BoundingBoxTransformedToParent.MaxX;
            float ballLeft = ballSprite.BoundingBoxTransformedToParent.MinX;

            float screenRight = VisibleBoundsWorldspace.MaxX;
            float screenLeft = VisibleBoundsWorldspace.MinX;

            bool shouldReflectXVelocity = (ballRight > screenRight && ballXVelocity > 0) || (ballLeft < screenLeft && ballXVelocity < 0);
            if (shouldReflectXVelocity)
            {
                ballXVelocity *= -1;
            }

            if (ballSprite.PositionY > VisibleBoundsWorldspace.MaxY)
            {
                ballSprite.PositionX = 640;
                ballSprite.PositionY = 320;

                ballYVelocity *= -1;
                score = 0;
                label.Text = "Score: 0";
            }
        }
    }
}

