using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using VideoPaintballCommon.MapObjects;
using VideoPaintballCommon.VPP;

namespace VideoPaintballCommon
{
    /// <summary>
    /// issue [B.2.2] of the design document
    /// </summary>
    public static class MapParser
    {
        /// <summary>
        /// Responsible for turning CS411 class specifications into our internal representation of the map (for our purposes the map = the gamestate, since there is no real "map" per se)
        /// </summary>
        /// <param name="dataFromServer"></param>
        /// <returns></returns>
        public static Map ParseMap(string mapData)
        {
            Map map = new Map();
            string[] mapObjects = mapData.Replace(">", string.Empty).Split('<');
            foreach (string mapObject in mapObjects)
            {
                if (mapObject != string.Empty)
                {
                    switch (mapObject[0])
                    {
                        case 'p':
                            Player player = ParsePlayer(mapObject.Split(','));
                            map.Players.Add(player.ID, player);
                            break;

                        case 'b':
                            map.Paintballs.Add(ParsePaintball(mapObject.Split(',')));
                            break;

                        case 'o':
                            map.Obstacles.Add(ParseObstacle(mapObject.Split(',')));
                            break;
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// [B.1.1] of the design document
        /// </summary>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        private static Player ParsePlayer(string[] mapObject)
        {
            Player player = new Player(ushort.Parse(mapObject[8]), mapObject[1]);
           player.Location = new PointF(float.Parse(mapObject[2]), float.Parse(mapObject[3]));
            player.FacingDirection = (VideoPaintballCommon.MapObjects.FacingDirectionType) Enum.Parse(typeof(VideoPaintballCommon.MapObjects.FacingDirectionType),(mapObject[4]));
            player.ShieldLocation = (VideoPaintballCommon.MapObjects.ShieldLocationType) Enum.Parse(typeof(VideoPaintballCommon.MapObjects.ShieldLocationType),mapObject[5]);
            player.Health = UInt16.Parse(mapObject[6]);
            player.Ammo = UInt16.Parse(mapObject[7]);

            return player;
        }

        /// <summary>
        /// [B.1.1] of the design document
        /// </summary>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        private static Paintball ParsePaintball(string[] mapObject)
        {
            float locationX = float.Parse(mapObject[1]);
            float locationY = float.Parse( mapObject[2]);
           Paintball paintball = new Paintball(new PointF(locationX, locationY));
           
            return paintball;
        }

        /// <summary>
        /// [B.1.1] of the design document
        /// </summary>
        /// <param name="mapObject"></param>
        /// <returns></returns>
        private static Obstacle ParseObstacle(string[] mapObject)
        {
            float locationX = float.Parse( mapObject[1] );
            float locationY = float.Parse(mapObject[2]);
            float width = float.Parse(mapObject[3]);
            float height = float.Parse(mapObject[4]);
            Obstacle obstacle = new Obstacle(new PointF(locationX, locationY), new SizeF(width, height));
            return obstacle;
        }
    }
}
