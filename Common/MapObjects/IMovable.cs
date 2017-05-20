using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace VideoPaintballCommon.MapObjects
{
    public interface IMovable
    {
        void Move();
        
       PointF Velocity
        {
            get;
            set;
        }
    }
}
