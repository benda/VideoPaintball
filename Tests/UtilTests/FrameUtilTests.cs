using System;

using NUnit.Framework;

using VideoPaintballClient.Util;

namespace VideoPaintballTests.UtilTests
{
    [TestFixture]
    public class FrameUtilTests
    {
        [Test]
        public void CalculateFrameRateTest()
        {
            int i = 0;
            int currentFrameRate = 0;
            Console.WriteLine("Running frame rate test 1, this takes 10 seconds...");
            while(i < 100)
            {
                currentFrameRate = FrameUtil.CalculateFrameRate();
                System.Threading.Thread.Sleep(100); //simulate rendering
                i++;
            }
           
            //frame rate should be 10, might be +-5
            Assert.Greater(currentFrameRate, 5);
            Assert.Less(currentFrameRate, 15);


            i = 0;
            currentFrameRate = 0;
            Console.WriteLine("Running frame rate test 2, this takes 10 seconds...");
            while (i < 1000)
            {
                currentFrameRate = FrameUtil.CalculateFrameRate();
                System.Threading.Thread.Sleep(10); //simulate rendering
                i++;
            }

            //frame rate should be 100, might be +- 50
            Assert.Greater(currentFrameRate, 50);
            Assert.Less(currentFrameRate, 150);

        }
    }
}