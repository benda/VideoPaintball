using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPaintballCommon.Detectors
{
   /// <summary>
   /// issue [B.2.3] of the design document
   /// </summary>
   public static class CollisionDetector
    {
       public static bool Collision(VideoPaintballCommon.MapObjects.MapObject mapObject1, VideoPaintballCommon.MapObjects.MapObject mapObject2) 
       { 
           return 
               !(mapObject1.Location.X > mapObject2.Location.X + mapObject2.Size.Width 
               || mapObject1.Location.X + mapObject1.Size.Width < mapObject2.Location.X 
               || mapObject1.Location.Y > mapObject2.Location.Y + mapObject2.Size.Height 
               || mapObject1.Location.Y + mapObject1.Size.Height < mapObject2.Location.Y); 
       }
    }
}
