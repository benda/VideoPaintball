using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using VideoPaintballCommon.VPP;
using VideoPaintballCommon.Util;

namespace VideoPaintballCommon.MapObjects
{
    public class AIPlayer : Player
    {
        private string _action;
        private int _turnsPassedSinceLastMove = 0;
        private Player _targetedPlayer = null;

        public AIPlayer(ushort teamNumber, string id)

            : base(teamNumber, id)
        {
        }


        public void DoTurnAction(Player[] players)
        {
            while (_targetedPlayer == null || _targetedPlayer.Health == 0 || _targetedPlayer.ID == this.ID)
            {
                Random random = new Random(); 
                _targetedPlayer = players[random.Next(0, players.Length)];
            }           

            /* this code chooses a human target instead
            foreach (Player player in players)
            {

                if (player is AIPlayer)
                {
                }
                else
                {
                    targetedPlayer = player;
                    break;
                }
            }
            */

            if (_turnsPassedSinceLastMove == 10)
            {
                if (_targetedPlayer.Location.Y - 10 > this.Location.Y /*+ DimensionsUtil.GetAvatarHeight() * 2*/)
                {
                    //target below ai player, check facing direction, then either move down to match Ys or if Xs line up, shoot at player
                    if (this.FacingDirection != FacingDirectionType.South)
                    {
                        _action = MessageConstants.PlayerActionRotateRight;
                    }
                    else if (this.ShieldLocation != ShieldLocationType.Back)
                    {
                        _action = MessageConstants.PlayerActionShieldBack;
                    }
                    else
                    {
                        if (_targetedPlayer.Location.X + DimensionsUtil.GetAvatarWidth() < this.Location.X || _targetedPlayer.Location.X > this.Location.X + DimensionsUtil.GetAvatarWidth())
                        {
                            //target below ai player, and not lined up with gun
                            _action = MessageConstants.PlayerActionDown;
                        }
                        else
                        {
                            _action = MessageConstants.PlayerActionShoot;
                        }
                    }
                }
                else if (_targetedPlayer.Location.Y + 10 < this.Location.Y /*- DimensionsUtil.GetAvatarHeight() * 2*/)
                {
                    //target above ai player, check facing direction, then either move up to match Ys or if Xs line up, shoot at player
                    if (this.FacingDirection != FacingDirectionType.North)
                    {
                        _action = MessageConstants.PlayerActionRotateRight;
                    }
                    else if (this.ShieldLocation != ShieldLocationType.Front)
                    {
                        _action = MessageConstants.PlayerActionShieldFront;
                    }
                    else
                    {
                        if (_targetedPlayer.Location.X < this.Location.X || _targetedPlayer.Location.X > this.Location.X + DimensionsUtil.GetAvatarWidth())
                        {
                            //target above ai player, and not lined up with gun
                            _action = MessageConstants.PlayerActionUp;
                        }
                        else
                        {
                                _action = MessageConstants.PlayerActionShoot;
                        }
                    }
                }
                else
                {
                    //target player on same y as ai player
                    if (_targetedPlayer.Location.X < this.Location.X)
                    {
                        //target player to the left of ai player
                        if (this.FacingDirection != FacingDirectionType.West)
                        {
                            _action = MessageConstants.PlayerActionRotateLeft;
                        }
                        else if (this.ShieldLocation != ShieldLocationType.Left)
                        {
                            _action = MessageConstants.PlayerActionShieldLeft;
                        }
                        else
                        {
                            _action = MessageConstants.PlayerActionShoot;
                        }
                    }
                    else
                    {
                        //target player to the right of ai player
                        if (this.FacingDirection != FacingDirectionType.East)
                        {
                            _action = MessageConstants.PlayerActionRotateRight;
                        }
                        else if (this.ShieldLocation != ShieldLocationType.Right)
                        {
                            _action = MessageConstants.PlayerActionShieldRight;
                        }
                        else
                        {
                            _action = MessageConstants.PlayerActionShoot;
                        }
                    }

                }
                _turnsPassedSinceLastMove = 0;
            }
            else
            {
                _turnsPassedSinceLastMove++;
                _action = MessageConstants.PlayerActionNone;
            }
        }

        public string Action
        {
            get { return _action; }
        }
    }
}
