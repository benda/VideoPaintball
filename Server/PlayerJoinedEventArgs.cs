using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using VideoPaintballCommon.Net;

namespace VideoPaintballServer
{
    class PlayerJoinedEventArgs : EventArgs
    {

        public PlayerJoinedEventArgs(string clientIP, NetworkCommunicator client)
        {
            ClientIP = clientIP;
            Client = client;
        }

        public NetworkCommunicator Client { get; private set; }
        public string ClientIP { get; private set; }

    }
}
