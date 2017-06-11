using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using VideoPaintballCommon.Util;

namespace VideoPaintballCommon.MapObjects
{
    public class Paintball : MapObject, IRenderable, IMovable
    {
      private PointF _velocity;

       public Paintball(PointF location, PointF velocity) : base(location)
       {
           this.Velocity = velocity;
           this.Size = new SizeF(DimensionsUtil.GetPaintballWidth(), DimensionsUtil.GetPaintballHeight());
       }

       public Paintball(PointF location) : base(location)
        {
            this.Size = new SizeF(DimensionsUtil.GetPaintballWidth(), DimensionsUtil.GetPaintballHeight());
        }

        public void Render(Graphics graphics)
        {
            Pen bluePen = new Pen(Color.Blue);
            SolidBrush brush = new SolidBrush(Color.Blue);
            graphics.FillRectangle(brush, Location.X, Location.Y, Size.Width, Size.Height);
        }       

       public override string ToString()
       {
           return "<b," + Location.X.ToString() + "," + Location.Y.ToString() + "," + "Normal>";
       }


       public void Move()
       {
           this.Location = new PointF(this.Location.X + this.Velocity.X, this.Location.Y + this.Velocity.Y);
       }

       public PointF Velocity
       {
           get { return _velocity; }
           set { _velocity = value; }
       }
    }
}
