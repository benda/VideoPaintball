using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using VideoPaintballCommon.Net;

namespace VideoPaintballClient
{
    public class GameConfiguration
    {
        public GameConfiguration() { }

        public NetworkCommunicator ServerConnection { get; set; }
        public IPAddress ServerIPAddress { get;  set; }
        public bool ThisClientIsServer { get; set; }
    }
}
