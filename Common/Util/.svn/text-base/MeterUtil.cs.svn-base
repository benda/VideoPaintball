using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VideoPaintballCommon.Util
{
    /// <summary>
    /// issue [A.1.1] in the design document
    /// </summary>
    public static class MeterUtil
    {
        public static Color GetColorFromValue(int value)
        {
            Color color = Color.Empty;

            if (value > 50)
            {
                color = Color.Green;
            }
            else if (value > 25)
            {
                color = Color.Yellow;
            }
            else if (value <= 25)
            {
                color = Color.Red;
            }

            return color;
        }
    }
}
