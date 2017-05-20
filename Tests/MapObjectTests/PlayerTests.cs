using System;
using System.Drawing;

using VideoPaintballCommon.MapObjects;

using NUnit.Framework;

namespace VideoPaintballTests.MapObjectTests
{
    [TestFixture]
    public class PlayerTests
    {
        private Player _player;

        [SetUp]
        protected void SetUp()
        {
          CreatePlayer();
        }

        [Test]
        public void PlayerMoveTest()
        {
            _player.Velocity = new PointF(5, 10);
            _player.Move();
            Assert.AreEqual(_player.Location.X, 5);
            Assert.AreEqual(_player.Location.Y, 10);
        }

        [Test]
        public void PlayerUndoMoveTest()
        {
            _player.Velocity = new PointF(5, 10);
            _player.Move();
            _player.UndoMove();
            Assert.AreEqual(_player.Location.X, 0);
            Assert.AreEqual(_player.Location.Y, 0);
        }

        [Test]
        public void PlayerRotateRightTest()
        {
            _player.FacingDirection = FacingDirectionType.North;
            _player.RotateRight();
            Assert.AreEqual(_player.FacingDirection, FacingDirectionType.East);
        }

        [Test]
        public void PlayerRotateLeftTest()
        {
            _player.FacingDirection = FacingDirectionType.West;
            _player.RotateLeft();
            Assert.AreEqual(_player.FacingDirection, FacingDirectionType.South);
        }

        [Test]
        public void PlayerMoveShieldToLeftTest()
        {
            _player.MoveShieldToLeft();
            Assert.AreEqual(_player.ShieldLocation, ShieldLocationType.Left);
        }

        [Test]
        public void PlayerMoveShieldToRightTest()
        {
            _player.MoveShieldToRight();
            Assert.AreEqual(_player.ShieldLocation, ShieldLocationType.Right);
        }

        [Test]
        public void PlayerMoveShieldToFrontTest()
        {
            _player.MoveShieldToFront();
            Assert.AreEqual(_player.ShieldLocation, ShieldLocationType.Front);
        }

        [Test]
        public void PlayerMoveShieldToBackTest()
        {
            _player.MoveShieldToBack();
            Assert.AreEqual(_player.ShieldLocation, ShieldLocationType.Back);
        }

        [Test]
        public void PlayerRecordPaintballHitTest()
        {
            _player.RecordPaintballHit();
            Assert.AreEqual(_player.Health, 95);
        }

        [Test]
        public void PlayerRecordPaintballFiredTest()
        {
            _player.RecordPaintballFired();
            Assert.AreEqual(_player.Ammo, 99);
        }

        [Test]
        public void PlayerToStringTest()
        {
            string text = _player.ToString();
            Assert.AreEqual(text, "<p,1,0,0,North,Front,100,100,5>");
        }

        [Test]
        public void CreatePlayer()
        {
            _player = new Player(5, "1");
            Assert.AreEqual(_player.Location.X, 0);
            Assert.AreEqual(_player.Location.Y, 0);
            Assert.AreEqual(_player.ID, "1");
            Assert.AreEqual(_player.TeamNumber, 5);
            Assert.AreEqual(_player.Location.X, 0);
            Assert.AreEqual(_player.Location.Y, 0);
            Assert.AreEqual(_player.Health, 100);
            Assert.AreEqual(_player.Ammo, 100);
            Assert.AreEqual(_player.ShieldLocation, ShieldLocationType.Front);
        }
    }
}
