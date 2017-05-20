using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

using VideoPaintballCommon.MapObjects;
using VideoPaintballCommon;

namespace VideoPaintballCommon
{
    /// <summary>
    /// issue [B.2.1] of the design document
    /// </summary>
    public static class MapLoader
    {

        public static Map LoadRandomMap()
        {
            string[] maps = Directory.GetFiles("..\\..\\..\\Maps");
            Random r = new Random();
            string chosenMap = maps[ r.Next(0, maps.Length) ];
            StreamReader reader = null;
            string mapData = string.Empty;
            try
            {
                reader = new StreamReader(chosenMap);
                mapData = reader.ReadToEnd().Replace("\n", string.Empty).Replace("\t", string.Empty).Replace("\r", string.Empty);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

           
            
           return MapParser.ParseMap(mapData);  
        }

       
    }
}
