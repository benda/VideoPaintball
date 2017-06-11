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
        public PlayerJoinedEventArgs(NetworkCommunicator client)
        {
            Client = client;
        }

        public NetworkCommunicator Client { get; private set; }
    }
}
