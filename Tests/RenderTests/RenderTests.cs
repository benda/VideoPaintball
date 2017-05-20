using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using VideoPaintballCommon.MapObjects;

using NUnit.Framework;

namespace VideoPaintballTests.RenderTests
{
    [TestFixture]
    public class RenderTests
    {
        [SetUp]
        protected void SetUp()
        {

        }


        [Test]
        public void RenderPlayerTest()
        {
            Player player = new Player(1, "1");
            player.Location = new PointF(100, 100);
            Bitmap actualImage = new Bitmap(800, 600);
            Graphics actualGraphics = Graphics.FromImage(actualImage);
            actualGraphics.Clear(Color.Black);
            player.Render(actualGraphics);
            actualImage.Save("../../RenderingActual/player.bmp");

            Bitmap expectedImage = (Bitmap) Bitmap.FromFile("../../RenderingExpected/player.bmp");

            VerifyPictureIsTheSame(expectedImage, actualImage);

            actualImage.Dispose();
            expectedImage.Dispose();
        }

        [Test]
        public void RenderPaintballTest()
        {
            Paintball paintball = new Paintball(new PointF(100, 100));
            Bitmap actualImage = new Bitmap(800, 600);
            Graphics actualGraphics = Graphics.FromImage(actualImage);
            actualGraphics.Clear(Color.Black);
            paintball.Render(actualGraphics);
            actualImage.Save("../../RenderingActual/paintball.bmp");

            Bitmap expectedImage = (Bitmap)Bitmap.FromFile("../../RenderingExpected/paintball.bmp");

            VerifyPictureIsTheSame(expectedImage, actualImage);

            actualImage.Dispose();
            expectedImage.Dispose();
        }

        [Test]
        public void RenderObstacleTest()
        {
            Obstacle obstacle = new Obstacle(new PointF(100, 100), new SizeF(40, 20));
            Bitmap actualImage = new Bitmap(800, 600);
            Graphics actualGraphics = Graphics.FromImage(actualImage);
            actualGraphics.Clear(Color.Black);
            obstacle.Render(actualGraphics);
            actualImage.Save("../../RenderingActual/obstacle.bmp");

            Bitmap expectedImage = (Bitmap)Bitmap.FromFile("../../RenderingExpected/obstacle.bmp");

            VerifyPictureIsTheSame(expectedImage, actualImage);

            actualImage.Dispose();
            expectedImage.Dispose();
        }

        [Test]
        public void RenderMapTest()
        {
            Map map = new Map();
  
            map.Obstacles.Add(new Obstacle(new PointF(400, 300), new SizeF(30, 50)));
            map.Obstacles.Add(new Obstacle(new PointF(202, 200), new SizeF(11, 22)));
            map.Obstacles.Add(new Obstacle(new PointF(321, 451), new SizeF(45, 86)));

            Player player1 = new Player(1, "1");
            player1.Location = new PointF(300, 300);

            Player player2 = new Player(2, "2");
            player2.Location = new PointF(500, 500);

            Player player3 = new Player(3, "3");
            player3.Location = new PointF(700, 500);

            map.Players.Add("1", player1);
            map.Players.Add("2", player2);
            map.Players.Add("3", player3);

            map.Paintballs.Add(new Paintball(new PointF(321, 410)));
            map.Paintballs.Add(new Paintball(new PointF(714, 325)));
            map.Paintballs.Add(new Paintball(new PointF(613, 43)));
            map.Paintballs.Add(new Paintball(new PointF(24, 200)));

            Bitmap actualImage = new Bitmap(800, 600);
            Graphics actualGraphics = Graphics.FromImage(actualImage);
            actualGraphics.Clear(Color.Black);

            map.Render(actualGraphics);

            actualImage.Save("../../RenderingActual/map.bmp");

            Bitmap expectedImage = (Bitmap)Bitmap.FromFile("../../RenderingExpected/map.bmp");

            VerifyPictureIsTheSame(expectedImage, actualImage);

            actualImage.Dispose();
            expectedImage.Dispose();
     
        }


        private void VerifyPictureIsTheSame(Bitmap expectedImage, Bitmap actualImage)
        {
            Color expectedPixel;
            Color actualPixel;
            for (int i = 0; i < expectedImage.Width; i++)
            {

                for (int j = 0; j < expectedImage.Height; j++)
                {
                    expectedPixel = expectedImage.GetPixel(i, j);
                    actualPixel = actualImage.GetPixel(i, j);
                    Assert.AreEqual(expectedPixel.ToString(), actualPixel.ToString());

                }
            }
        }
    }
}
