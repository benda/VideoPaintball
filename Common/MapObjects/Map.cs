using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using VideoPaintballCommon.VPP;
using VideoPaintballCommon.Detectors;
using VideoPaintballCommon.Util;
using VideoPaintballCommon;

namespace VideoPaintballCommon.MapObjects
{
    public class Map
    {
        private Dictionary<string, Player> _players = new Dictionary<string, Player>();
        private List<Paintball> _paintballs = new List<Paintball>();
        private List<Berry> _berries = new List<Berry>();
        private List<Obstacle> _obstacles = new List<Obstacle>();
        private List<Paintball> _paintballsToRemove = new List<Paintball>();
        private SizeF _size;

        public Map()
        {
            Size = new SizeF(DimensionsUtil.GetMapWidth(), DimensionsUtil.GetMapHeight());
        }

        /// <summary>
        /// issue [A.1.2] of the design document
        /// </summary>
        /// <param name="graphics"></param>
        public void Render(Graphics graphics)
        {
            graphics.Clear(Color.Black);

            //TODO: just have 1 list of irenderables instead?
            foreach (Player player in Players.Values)
            {
                player.Render(graphics);
            }

            foreach (Paintball paintball in Paintballs)
            {
                paintball.Render(graphics);
            }

            foreach (Berry berry in Berries)
            {
                berry.Render(graphics);
            }

            foreach (Obstacle obstacle in Obstacles)
            {
                obstacle.Render(graphics);
            }
        }

        public void StartNewGame(List<PlayerAction> playerActions)
        {
            Obstacles.Clear();
            Paintballs.Clear();

            Map map = MapLoader.LoadRandomMap();
            Obstacles = map.Obstacles;
            Berries = map.Berries;

            if (_players.Count == 1)
            {
                AIPlayer player = null;
                string id = string.Empty;
                for (int i = 2; i <= 6; i++)
                {
                    id = "AI" + i.ToString();
                    player = new AIPlayer((ushort)i, id);
                    player.Location = PlacePlayer();
                    Players.Add(id, player);
                    playerActions.Add(new PlayerAction(id, MessageConstants.PlayerActionNone));
                }
            }

            foreach (Player player in Players.Values)
            {
                player.Location = PlacePlayer();
                player.Health = 100;
                player.Ammo = 100;
            }
        }

        public void Update(List<PlayerAction> playerActions)
        {
            //restart game if only 1 active player
            int activePlayers = 0;
            foreach (Player player in Players.Values)
            {
                if (player.Health > 0 && player.Ammo > 0)
                {
                    activePlayers++;
                }
            }

            if (activePlayers == 1)
            {
                StartNewGame(playerActions);
                return;
            }


            //AI players take their turn here
            Player[] players = new Player[Players.Count];
            Players.Values.CopyTo(players, 0);

            foreach (Player player in Players.Values)
            {
                if (player is AIPlayer)
                {
                    ((AIPlayer)player).DoTurnAction(players);

                    foreach (PlayerAction action in playerActions)
                    {
                        if (action.PlayerID == player.ID)
                        {
                            action.Action = ((AIPlayer)player).Action;
                        }
                    }
                }
            }

            foreach (PlayerAction playerAction in playerActions)
            {
                Player player = Players[playerAction.PlayerID];
                if (player.Health > 0)
                {
                    //set velocities on players, etc.
                    switch (playerAction.Action)
                    {
                        case MessageConstants.PlayerActionDown:
                            player.Velocity = new PointF(0, 4);
                            break;

                        case MessageConstants.PlayerActionLeft:
                            player.Velocity = new PointF(-4, 0);
                            break;

                        case MessageConstants.PlayerActionRight:
                            player.Velocity = new PointF(4, 0);
                            break;

                        case MessageConstants.PlayerActionUp:
                            player.Velocity = new PointF(0, -4);
                            break;

                        case MessageConstants.PlayerActionRotateLeft:
                            player.RotateLeft();
                            break;

                        case MessageConstants.PlayerActionRotateRight:
                            player.RotateRight();
                            break;

                        case MessageConstants.PlayerActionShieldBack:
                            player.MoveShieldToBack();
                            break;

                        case MessageConstants.PlayerActionShieldFront:
                            player.MoveShieldToFront();
                            break;

                        case MessageConstants.PlayerActionShieldLeft:
                            player.MoveShieldToLeft();
                            break;

                        case MessageConstants.PlayerActionShieldRight:
                            player.MoveShieldToRight();
                            break;

                        case MessageConstants.PlayerActionShoot:
                            if (player.Ammo > 0)
                            {
                                PointF velocity = new PointF(0, 0);
                                PointF location = new PointF(0, 0);

                                switch (player.FacingDirection)
                                {
                                    case FacingDirectionType.North:
                                        velocity = new PointF(0, -4);
                                        location = new PointF(player.Location.X + (DimensionsUtil.GetAvatarWidth() / 2) - (DimensionsUtil.GetPaintballWidth() / 2), Players[playerAction.PlayerID].Location.Y - DimensionsUtil.GetGunHeight() - DimensionsUtil.GetPaintballOffset());
                                        break;

                                    case FacingDirectionType.South:
                                        velocity = new PointF(0, 4);
                                        location = new PointF(player.Location.X + (DimensionsUtil.GetAvatarWidth() / 2) - (DimensionsUtil.GetPaintballWidth() / 2), Players[playerAction.PlayerID].Location.Y + DimensionsUtil.GetPaintballOffset() + DimensionsUtil.GetAvatarHeight());
                                        break;

                                    case FacingDirectionType.East:
                                        velocity = new PointF(4, 0);
                                        location = new PointF(player.Location.X + DimensionsUtil.GetPaintballOffset() + DimensionsUtil.GetAvatarWidth(), Players[playerAction.PlayerID].Location.Y + (DimensionsUtil.GetAvatarHeight() / 2) - (DimensionsUtil.GetPaintballHeight() / 2));
                                        break;

                                    case FacingDirectionType.West:
                                        velocity = new PointF(-4, 0);
                                        location = new PointF(player.Location.X - DimensionsUtil.GetPaintballOffset(), Players[playerAction.PlayerID].Location.Y + (DimensionsUtil.GetAvatarHeight() / 2) - (DimensionsUtil.GetPaintballHeight() / 2));
                                        break;
                                }

                                Paintballs.Add(new Paintball(location, velocity));
                                player.RecordPaintballFired();
                            }
                            break;

                        case MessageConstants.PlayerActionPowerShoot:

                            break;

                        case MessageConstants.PlayerActionNone:

                            break;
                    }
                }
            }


            //move all objects
            foreach (Player player in Players.Values)
            {
                player.Move();
                player.Velocity = new PointF(0, 0);
            }

            foreach (Paintball paintball in Paintballs)
            {
                paintball.Move();
            }




            //detect and record collisions between paintballs and players
            //issue [B.2.6] of the design document
            Shield shield = null;
            PointF shieldLocation = new PointF(0, 0);
            SizeF shieldSize = new SizeF(0, 0);
            bool wasCollision = false;
            foreach (Paintball paintball in Paintballs)
            {
                wasCollision = false;

                foreach (Player player in Players.Values)
                {
                    if (player.Health > 0)
                    {
                        switch (player.ShieldLocation)
                        {
                            case ShieldLocationType.Front:
                                shieldLocation = new PointF(player.Location.X, player.Location.Y - DimensionsUtil.GetAvatarShieldOffset());
                                shieldSize = new SizeF(DimensionsUtil.GetAvatarWidth(), 1);
                                break;

                            case ShieldLocationType.Back:
                                shieldLocation = new PointF(player.Location.X, player.Location.Y + DimensionsUtil.GetAvatarHeight() + DimensionsUtil.GetAvatarShieldOffset());
                                shieldSize = new SizeF(DimensionsUtil.GetAvatarWidth(), 1);
                                break;

                            case ShieldLocationType.Left:
                                shieldLocation = new PointF(player.Location.X - DimensionsUtil.GetAvatarShieldOffset(), player.Location.Y);
                                shieldSize = new SizeF(1, DimensionsUtil.GetAvatarHeight());
                                break;

                            case ShieldLocationType.Right:
                                shieldLocation = new PointF(player.Location.X + DimensionsUtil.GetAvatarWidth() + DimensionsUtil.GetAvatarShieldOffset(), player.Location.Y);
                                shieldSize = new SizeF(1, DimensionsUtil.GetAvatarHeight());
                                break;
                        }

                        shield = new Shield(shieldLocation, shieldSize);
                        if (CollisionDetector.Collision(paintball, shield))
                        {
                            wasCollision = true;
                            _paintballsToRemove.Add(paintball);
                        }
                        else if (CollisionDetector.Collision(paintball, player))
                        {
                            wasCollision = true;
                            player.RecordPaintballHit();
                            _paintballsToRemove.Add(paintball);
                        }

                        if (wasCollision)
                        {
                            break;
                        }
                    }
                }

                if (!wasCollision)
                {
                    foreach (Obstacle obstacle in Obstacles)
                    {
                        if (CollisionDetector.Collision(paintball, obstacle))
                        {
                            wasCollision = true;
                            _paintballsToRemove.Add(paintball);
                        }
                    }
                }

                if (!wasCollision)
                {
                    if (OffscreenDetector.OffScreen(paintball))
                    {
                        _paintballsToRemove.Add(paintball);
                    }
                }
            }



            //remove paintballs that hit players, obstacles or went offscreen
            foreach (Paintball paintball in _paintballsToRemove)
            {
                Paintballs.Remove(paintball);
            }
            _paintballsToRemove.Clear();



            //don't allow players to move over other players or over obstacles, or off-screen
            //issue [B.2.5] of the design document
            bool wasUndoed = false;
            foreach (Player player in Players.Values)
            {
                wasUndoed = false;
                if (OffscreenDetector.OffScreen(player))
                {
                    player.UndoMove();
                }
                else
                {
                    foreach (Obstacle obstacle in Obstacles)
                    {
                        if (CollisionDetector.Collision(player, obstacle))
                        {
                            wasUndoed = true;
                            player.UndoMove();
                        }

                        if (wasUndoed)
                        {
                            break;
                        }
                    }

                    if (!wasUndoed)
                    {
                        foreach (Player playerB in Players.Values)
                        {
                            if (playerB.Health > 0) //issue [B.2.7] of the design document
                            {
                                if (CollisionDetector.Collision(player, playerB) && player.ID != playerB.ID)
                                {
                                    wasUndoed = true;
                                    player.UndoMove();
                                }

                                if (wasUndoed)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

        }



        private PointF PlacePlayer()
        {
            bool found = false;
            Player dummyPlayer = new Player(1, "0");
            Random random = new Random();
            PointF loc = new PointF();

            while (!found)
            {
                loc.X = random.Next(0, 840 - (int)DimensionsUtil.GetAvatarWidth());
                loc.Y = random.Next(0, 540 - (int)DimensionsUtil.GetAvatarHeight());
                dummyPlayer.Location = loc;
                found = true;
                foreach (Player playerB in Players.Values)
                {
                    found = !CollisionDetector.Collision(dummyPlayer, playerB);
                    if (!found)
                        break;
                }
                if (found)
                {
                    foreach (Obstacle obstacle in Obstacles)
                    {
                        found = !CollisionDetector.Collision(dummyPlayer, obstacle);
                        if (!found)
                            break;
                    }
                }
            }

            return loc;
        }


        public override string ToString()
        {
            StringBuilder text = new StringBuilder();

            foreach (Player player in Players.Values)
            {

                text.Append(player.ToString());
            }

            foreach (Paintball paintball in Paintballs)
            {
                text.Append(paintball.ToString());
            }

            foreach (Obstacle obstacle in Obstacles)
            {
                text.Append(obstacle.ToString());
            }

            return text.ToString();
        }

        public SizeF Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public Dictionary<string, Player> Players
        {
            get { return _players; }
            set { _players = value; }
        }

        public List<Paintball> Paintballs
        {
            get { return _paintballs; }
            set { _paintballs = value; }
        }

        public List<Obstacle> Obstacles
        {
            get { return _obstacles; }
            set { _obstacles = value; }
        }

        public List<Berry> Berries
        {
            get { return _berries; }
            set { _berries = value; }
        }

    }
}
