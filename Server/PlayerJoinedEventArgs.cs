using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace VideoPaintballServer
{
    class PlayerJoinedEventArgs : EventArgs
    {

        public PlayerJoinedEventArgs(string clientIP, TcpClient tcpClient)
        {
            ClientIP = clientIP;
            TcpClient = tcpClient;
        }

        public TcpClient TcpClient { get; private set; }
        public string ClientIP { get; private set; }

    }
}
