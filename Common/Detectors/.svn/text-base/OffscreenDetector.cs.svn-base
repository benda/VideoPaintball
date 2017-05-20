using System;
using System.Collections.Generic;
using System.Text;

using VideoPaintballCommon.Util;

namespace VideoPaintballCommon.Detectors
{
    /// <summary>
    /// issue [B.2.4] of the design documents
    /// </summary>
    public static class OffscreenDetector
    {

        public static bool OffScreen(VideoPaintballCommon.MapObjects.MapObject mapObject)
        {
            return (mapObject.Location.X < 0 || mapObject.Location.Y < 0 || mapObject.Location.X > DimensionsUtil.GetMapWidth() || mapObject.Location.Y > DimensionsUtil.GetMapHeight());
        }
    }
}
