using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VideoPaintballCommon.MapObjects
{
    public class Berry : MapObject, IRenderable
    {
        public Berry(PointF location) : base(location) { }

        public void Render(Graphics graphics)
        {

        }
    }
}
