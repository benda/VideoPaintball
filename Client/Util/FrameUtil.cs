using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPaintballClient.Util
{
    /// <summary>
    /// 
    /// </summary>
  public static class FrameUtil
    {
        private static int lastTick;
        private static int lastFrameRate;
        private static int frameRate;

        /// <summary>
        /// issue [B.1.3] in the design document
        /// </summary>
        /// <returns></returns>
        public static int CalculateFrameRate()
        {
            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }


    }
}
