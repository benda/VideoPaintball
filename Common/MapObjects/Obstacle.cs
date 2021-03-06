using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using VideoPaintballCommon.Util;

namespace VideoPaintballCommon.MapObjects
{
    public class Obstacle : MapObject, IRenderable
    {
        public Obstacle(PointF location, SizeF size) : base(location)
        {
            this.Size = new SizeF(DimensionsUtil.GetObstacleWidth(size.Width), DimensionsUtil.GetObstacleHeight(size.Height));
        }

        public override string ToString()
        {
            return "<o," + Location.X.ToString() + "," + Location.Y.ToString() + "," + Size.Width.ToString() + "," + Size.Height.ToString() + ">";
        }

        public void Render(Graphics graphics)
        {
            Pen orangePen = new Pen(Color.Orange);
            SolidBrush brush = new SolidBrush(Color.Orange);
            graphics.FillRectangle(brush, Location.X, Location.Y, Size.Width, Size.Height);
        }
    }
}
