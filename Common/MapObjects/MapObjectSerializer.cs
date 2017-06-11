﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoPaintballCommon.MapObjects
{
    public class MapObjectSerializer
    {
        StringBuilder _serialized = new StringBuilder();

        public void AppendProperty(object value)
        {
            _serialized.Append(value);
            _serialized.Append(",");
        }
    }
}
