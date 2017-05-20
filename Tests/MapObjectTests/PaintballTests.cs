using System;
using System.Drawing;

using VideoPaintballCommon.MapObjects;

using NUnit.Framework;

namespace VideoPaintballTests.MapObjectTests
{
   [TestFixture]
   public class PaintballTests
    {
        private Paintball _paintball;

        [SetUp]
        protected void SetUp()
        {
            CreatePaintball();
        }

       [Test]
       public void PaintballMoveTest()
       {
           _paintball.Move();
           Assert.AreEqual(_paintball.Location.X, 115);
           Assert.AreEqual(_paintball.Location.Y, 105);
       }

       [Test]
       public void PaintballToStringTest()
       {
          string text = _paintball.ToString();
          Assert.AreEqual(text, "<b,100,100,Normal>");
       }

       [Test]
       public void CreatePaintball()
       {
           PointF location = new PointF(100, 100);
           PointF velocity = new PointF(15, 5);

           _paintball = new Paintball(location, velocity);
           Assert.AreEqual(_paintball.Location.X, 100);
           Assert.AreEqual(_paintball.Location.Y, 100);

           Assert.AreEqual(_paintball.Velocity.X, 15);
           Assert.AreEqual(_paintball.Velocity.Y, 5);
       }

    }
}
