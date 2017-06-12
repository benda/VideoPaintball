using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VideoPaintballCommon.MapObjects
{
    public class MapObject
    {
        private PointF _location;
        private SizeF _size;

        public MapObject(PointF location) : this(location, new SizeF())
        {
        }

        public MapObject(PointF location, SizeF size)
        {
            this.Location = location;
            this.Size = size;
            RenderTimes = 1;
        }
      
        public PointF Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public SizeF Size
        {
            get { return _size; }
            set { _size = value; }
        }    

        public int RenderTimes { get; set; }

        public virtual void Serialize(MapObjectSerializer serializer)
        {
            serializer.AppendProperty(Location.X);
            serializer.AppendProperty(Location.Y);
            serializer.AppendProperty(Size.Width);
            serializer.AppendProperty(Size.Height);
        }
    }
}
