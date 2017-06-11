using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using VideoPaintballCommon.MapObjects;
using VideoPaintballCommon.VPP;

namespace VideoPaintballCommon
{
    public static class MapParser
    {
        public static Map ParseMap(string mapData)
        {
            Map map = new Map();
            string[] mapObjects = mapData.Replace(">", string.Empty).Split(new char[] { '<' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string mapObject in mapObjects)
            {
                string[] mapObjectItems = mapObject.Split(',');
                switch (mapObjectItems[0])
                {
                    case "p":
                        Player player = ParsePlayer(mapObjectItems);
                        map.Players.Add(player.ID, player);
                        break;

                    case "b":
                        map.Paintballs.Add(ParsePaintball(mapObjectItems));
                        break;

                    case "o":
                        map.Obstacles.Add(ParseObstacle(mapObjectItems));
                        break;
                    case "ph":
                        map.PaintballHits.Add(ParsePaintballHit(mapObjectItems));
                        break;
                }
            }

            return map;
        }

        private static Player ParsePlayer(string[] mapObject)
        {
            Player player = new Player(ushort.Parse(mapObject[8]), mapObject[1]);
            player.Location = new PointF(float.Parse(mapObject[2]), float.Parse(mapObject[3]));
            player.FacingDirection = (FacingDirectionType)Enum.Parse(typeof(FacingDirectionType), (mapObject[4]));
            player.ShieldLocation = (ShieldLocationType)Enum.Parse(typeof(ShieldLocationType), mapObject[5]);
            player.Health = UInt16.Parse(mapObject[6]);
            player.Ammo = UInt16.Parse(mapObject[7]);

            return player;
        }

        public static void SerializerPlayer(Player player, StringBuilder serialized)
        {
            serialized.Append("<p,");
            serialized.Append(player.ID);
            serialized.Append(",");
            serialized.Append(player.Location.X.ToString());
            serialized.Append(",");
            serialized.Append(player.Location.Y.ToString());
            serialized.Append(",");
            serialized.Append(player.FacingDirection.ToString());
            serialized.Append(",");
            serialized.Append(player.ShieldLocation.ToString());
            serialized.Append(",");
            serialized.Append(player.Health.ToString());
            serialized.Append(",");
            serialized.Append(player.Ammo.ToString());
            serialized.Append(",");
            serialized.Append(player.TeamNumber.ToString());
            serialized.Append(">");
        }

        private static Paintball ParsePaintball(string[] mapObject)
        {
            float locationX = float.Parse(mapObject[1]);
            float locationY = float.Parse(mapObject[2]);
            Paintball paintball = new Paintball(new PointF(locationX, locationY));

            return paintball;
        }

        private static Obstacle ParseObstacle(string[] mapObject)
        {
            float locationX = float.Parse(mapObject[1]);
            float locationY = float.Parse(mapObject[2]);
            float width = float.Parse(mapObject[3]);
            float height = float.Parse(mapObject[4]);
            Obstacle obstacle = new Obstacle(new PointF(locationX, locationY), new SizeF(width, height));
            return obstacle;
        }

        private static PaintballHit ParsePaintballHit(string[] mapObjectItems)
        {
            float locationX = float.Parse(mapObjectItems[1]);
            float locationY = float.Parse(mapObjectItems[2]);
      
            return new PaintballHit(new PointF(locationX, locationY));
        }

        private static void SerializePaintballHit(PaintballHit ph, StringBuilder serialized)
        {
            serialized.Append("<ph,");
            serialized.Append(ph.Location.X.ToString());
            serialized.Append(",");
            serialized.Append(ph.Location.Y.ToString());
            serialized.Append(",");
            serialized.Append(ph.Size.Width.ToString());
            serialized.Append(",");
            serialized.Append(ph.Size.Height.ToString());
            serialized.Append(">");
        }
    }
}
