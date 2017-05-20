using System;
using System.Drawing;
using NUnit.Framework;

using VideoPaintballCommon.Util;

namespace VideoPaintballTests.UtilTests
{
    [TestFixture]
    public class MeterUtilTests
    {
        [Test]
        public void GreenTest()
        {
          Assert.AreEqual(Color.Green, MeterUtil.GetColorFromValue(100));
          Assert.AreEqual(Color.Green, MeterUtil.GetColorFromValue(90));
          Assert.AreEqual(Color.Green, MeterUtil.GetColorFromValue(80));
          Assert.AreEqual(Color.Green, MeterUtil.GetColorFromValue(70));
          Assert.AreEqual(Color.Green, MeterUtil.GetColorFromValue(51));
        }

        [Test]
        public void YellowTest()
        {
            Assert.AreEqual(Color.Yellow, MeterUtil.GetColorFromValue(40));
            Assert.AreEqual(Color.Yellow, MeterUtil.GetColorFromValue(35));
            Assert.AreEqual(Color.Yellow, MeterUtil.GetColorFromValue(30));
            Assert.AreEqual(Color.Yellow, MeterUtil.GetColorFromValue(26));
            Assert.AreEqual(Color.Yellow, MeterUtil.GetColorFromValue(35));
        }

        [Test]
        public void RedTest()
        {
            Assert.AreEqual(Color.Red, MeterUtil.GetColorFromValue(10));
            Assert.AreEqual(Color.Red, MeterUtil.GetColorFromValue(5));
            Assert.AreEqual(Color.Red, MeterUtil.GetColorFromValue(8));
            Assert.AreEqual(Color.Red, MeterUtil.GetColorFromValue(20));
            Assert.AreEqual(Color.Red, MeterUtil.GetColorFromValue(25));
        }



    }
}
