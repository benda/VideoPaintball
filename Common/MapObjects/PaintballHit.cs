using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using VideoPaintballCommon.Util;

namespace VideoPaintballCommon.MapObjects
{
    public class PaintballHit : MapObject, IRenderable
    {
        private int MaxRenderTimes = 20;
        private int HalfMaxRenderTimes = 10;

        public PaintballHit(PointF location) : base(location)
        {
            RenderTimes = 0;          
        }
        
        public void Render(Graphics graphics)
        {
            if (RenderTimes <= MaxRenderTimes)
            {
                Pen pen = new Pen(Color.SandyBrown, 10F);
                if (RenderTimes <= HalfMaxRenderTimes)
                {
                    graphics.DrawEllipse(pen, Location.X, Location.Y, RenderTimes, RenderTimes);
                }
                else
                {
                    graphics.DrawEllipse(pen, Location.X, Location.Y, MaxRenderTimes - RenderTimes, MaxRenderTimes - RenderTimes);
                }

                RenderTimes++;
            }            
        }
    }
}
