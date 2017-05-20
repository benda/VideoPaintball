using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace VideoPaintballClient
{
    public class GameConfiguration
    {
        public GameConfiguration() { }

        public TcpClient ServerConnection { get; set; }
        public string ServerIPAddress { get;  set; }
        public bool ThisClientIsServer { get; set; }
    }
}
