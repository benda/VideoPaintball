using System;
using System.Drawing;

using VideoPaintballCommon.MapObjects;

using NUnit.Framework;

namespace VideoPaintballTests.MapObjectTests
{
    [TestFixture]
    public class MapTests
    {
        private Map _map;

        [SetUp]
        protected void SetUp()
        {
            CreateMap();
        }

        [Test]
        public void MapToStringTest()
        {
            string text = _map.ToString();
            Assert.AreEqual(text, "<p,1,0,0,North,Front,100,100,1><p,1,0,0,North,Front,100,100,1><p,3,0,0,North,Front,100,100,1><b,100,100,Normal><b,500,200,Normal><b,400,200,Normal>");
        }

        [Test]
        public void CreateMap()
        {
            _map = new Map();
            Assert.IsEmpty(_map.Players);
            Assert.IsEmpty(_map.Paintballs);
            Assert.IsEmpty(_map.Obstacles);
            Assert.IsEmpty(_map.Berries);
        
            _map.Players.Add("1", new Player(1, "1"));
            _map.Players.Add("2", new Player(1, "1"));
            _map.Players.Add("3", new Player(1, "3"));

            Assert.AreEqual(_map.Players.Count, 3);

            _map.Berries.Add(new Berry(new PointF(100,100)));
            Assert.AreEqual(_map.Berries.Count, 1);

            _map.Paintballs.Add(new Paintball(new PointF(100, 100), new PointF(10, 15)));
            _map.Paintballs.Add(new Paintball(new PointF(500, 200), new PointF(3, 5)));
            _map.Paintballs.Add(new Paintball(new PointF(400, 200), new PointF(4, 1)));

            Assert.AreEqual(_map.Paintballs.Count, 3);
        }
    }
}
