using System;
using System.Collections.Generic;
using System.Text;

namespace VideoPaintballCommon.VPP
{
    public class MessageConstants
    { 
        public const string MessageEndDelimiter = "#";
        public const string JoinGame = "JOIN";
        public const string CloseConnection = "CLOSE";
        public const string ServerAvailable = "A";
        public const string StartGame = "STARTGAME";
        public const string GameStarting = "GAMESTARTING";
        public const string PlayerJoined = "PlayerJoined";

        public const string PlayerActionUp = "UP";
        public const string PlayerActionDown = "DOWN";
        public const string PlayerActionLeft = "LEFT";
        public const string PlayerActionRight = "RIGHT";
        public const string PlayerActionRotateLeft = "ROTATELEFT";
        public const string PlayerActionRotateRight = "ROTATERIGHT";
        public const string PlayerActionShoot = "SHOOT";
        public const string PlayerActionPowerShoot = "POWERSHOOT";
        public const string PlayerActionShieldLeft = "SHIELDLEFT";
        public const string PlayerActionShieldRight = "SHIELDRIGHT";
        public const string PlayerActionShieldFront = "SHIELDFRONT";
        public const string PlayerActionShieldBack = "SHIELDBACK";
        public const string PlayerActionNone = "NONE";

    }
}
