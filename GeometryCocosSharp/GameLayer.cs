using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace GeometryCocosSharp
{
    public class GameLayer : CCLayerColor
    {

        // Define a label variable
        CCLabel testLabel;
        CCDrawNode drawNode;
        float lastPositionX, lastPositionY;
        byte r, g, b;
        //List<CCDrawNode> drawNodes;

        public GameLayer() : base(CCColor4B.LightGray)
        {
            testLabel = new CCLabel("Touches: 0", "Arial", 20, CCLabelFormat.SystemFont);
            testLabel.PositionX = 50;
            testLabel.PositionY = 1000;
            testLabel.Color = CCColor3B.Black;
            testLabel.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(testLabel);
            //drawNodes = new List<CCDrawNode>();

            drawNode = new CCDrawNode();
            // Origin is bottom-left of the screen. This moves
            // the drawNode 100 pixels to the right and 100 pixels up
            drawNode.PositionX = 0;
            drawNode.PositionY = 0;
            lastPositionX = drawNode.PositionX;
            lastPositionY = drawNode.PositionY;
            

            //drawNode.DrawCircle(
            //    center: new CCPoint(0, 0),
            //    radius: 20,
            //    color: CCColor4B.White);

            this.AddChild(drawNode);
        }

        void PaintCircle(float xPos, float yPos)
        {
            drawNode.DrawSolidCircle(
                pos: new CCPoint(xPos, yPos),
                radius: 20,
                color: new CCColor4B(200, 100, 100));
        }

        void PointAndCross(float xPos, float yPos)
        {
            drawNode.Clear();
            drawNode.DrawSolidCircle(
                pos: new CCPoint(xPos, yPos),
                radius: 70,
                color: new CCColor4B(250, 100, 100));
            drawNode.DrawLine(
                from: new CCPoint(xPos - 150, yPos),
                to: new CCPoint(xPos + 150, yPos),
                lineWidth: 2,
                lineCap: CCLineCap.Square,
                color: new CCColor4B(0, 0, 0));
            drawNode.DrawLine(
                from: new CCPoint(xPos, yPos - 150),
                to: new CCPoint(xPos, yPos + 150),
                lineWidth: 2,
                lineCap: CCLineCap.Square,
                color: new CCColor4B(0, 0, 0));
        }

        void PaintLineOnPoints(float currentPosX, float currentPosY, float lastPosX, float lastPosY)
        {
            drawNode.DrawLine(
                from: new CCPoint(lastPositionX, lastPositionY),
                to: new CCPoint(currentPosX, currentPosY),
                lineWidth: 10,
                lineCap: CCLineCap.Round,
                color: new CCColor4B(RandomColor(), RandomColor(), RandomColor()));
        }

        void PaintLineRandomColors(float currentPosX, float currentPosY, float lastPosX, float lastPosY)
        {
            CCV3F_C4B[] verts = new CCV3F_C4B[]
            {
                new CCV3F_C4B( new CCPoint(lastPositionX,lastPositionY), new CCColor4B(RandomColor(), RandomColor(), RandomColor())),
                new CCV3F_C4B( new CCPoint(currentPosX,currentPosY), new CCColor4B(RandomColor(), RandomColor(), RandomColor()))
            };

            drawNode.DrawLineList(verts);
        }

        void PaintCurve(float currentPosX, float currentPosY, float lastPosX, float lastPosY)
        {
            drawNode.DrawCubicBezier(
                    origin: new CCPoint(lastPositionX, lastPositionY),
                    control1: new CCPoint(lastPositionX + 30, lastPositionY + 30),
                    control2: new CCPoint(currentPosX - 30, currentPosY - 30),
                    destination: new CCPoint(currentPosX, currentPosY),
                    segments: 12,
                    lineWidth: 15,
                    color: new CCColor4B(r, g, b));
        }

        byte RandomColor()
        {
            Random random = new Random();
            int getRandom = random.Next(0, 255);

            return (byte)getRandom;
        }

        void GetRGBRandom()
        {
            r = RandomColor();
            g = RandomColor();
            b = RandomColor();
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // position the label on the center of the screen
            testLabel.Position = bounds.Center;

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesBegan = OnTouchBegan;
            touchListener.OnTouchesEnded = OnTouchesEnded;
            touchListener.OnTouchesMoved = HandleTouchesMoved;
            AddEventListener(touchListener, this);
        }

        private void OnTouchBegan(List<CCTouch> touches, CCEvent touchesEvent)
        {
            lastPositionX = touches[0].Location.X;
            lastPositionY = touches[0].Location.Y;
            testLabel.Text = "Touches: 0" + touches.Count.ToString();
        }

        private void HandleTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            var locationOnScreen = touches[0].Location;
            //drawNode.PositionX = locationOnScreen.X;
            //drawNode.PositionY = locationOnScreen.Y;
            //PaintCircle(locationOnScreen.X, locationOnScreen.Y);

            //PointAndCross(locationOnScreen.X, locationOnScreen.Y);


            PaintCurve(locationOnScreen.X, locationOnScreen.Y, lastPositionX, lastPositionY);

            //foreach (CCTouch touch in touches)
            //{
            //    PointAndCross(touches[touch.Id - 1].Location.X, touches[touch.Id - 1].Location.Y);
            //}

            //float middlePointX = 0f;
            //float middlePointY = 0f;
            //float getBetweenPointsX = (locationOnScreen.X - lastPositionX) / 10;
            //float getBetweenPointsY = (locationOnScreen.Y - lastPositionY) / 10;
            //int num = 0;

            //while (num < 10)
            //{

            //    //middlePointX = (locationOnScreen.X + lastPositionX) / 2;
            //    //middlePointY = (locationOnScreen.Y + lastPositionY) / 2;
            //    PaintCircle(locationOnScreen.X + (getBetweenPointsX), locationOnScreen.Y + (getBetweenPointsY));
            //    num++;
            //}
            //PaintLineOnPoints(locationOnScreen.X, locationOnScreen.Y, lastPositionX, lastPositionY);
            //PaintLineRandomColors(locationOnScreen.X, locationOnScreen.Y, lastPositionX, lastPositionY);
            lastPositionX = locationOnScreen.X;
            lastPositionY = locationOnScreen.Y;
            //testLabel.Text = "Touches: " + touches.Count.ToString();
        }

        void RunGameLogic(float frameTimeInSeconds)
        {

        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                // Perform touch handling here
                //drawNode.Clear();
                GetRGBRandom();
                //foreach (CCTouch location in touches)
                //{
                //    if (touches.Count -1 == location.Id)
                //    {
                //        drawNode.PositionX = location.Location.X;
                //        drawNode.PositionY = location.Location.Y;
                //        break;
                //    }
                //}
            }
        }
    }
}

