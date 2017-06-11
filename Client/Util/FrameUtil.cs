using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPaintballClient.Util
{
    public static class FrameUtil
    {
        private static int lastTick;
        private static int lastFrameRate;
        private static int frameRate;

        public static int CalculateRatePerSecond()
        {
            if (Math.Abs(Environment.TickCount - lastTick) >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }
    }
}
