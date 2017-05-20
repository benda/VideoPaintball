using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;

namespace VideoPaintballClient.Net.NetScanning
{
    public class NetScanPingCompletedEventArgs : EventArgs
    {
        private PingReply _reply = null;
        private bool _serverIsHostingGame = false;

        public NetScanPingCompletedEventArgs()
        {

        }

        public bool ServerIsHostingGame
        {
            get { return _serverIsHostingGame; }
            set { _serverIsHostingGame = value; }
        }
        
        public PingReply Reply
        {
            get { return _reply; }
            set{ _reply = value; }
        }

    }
}
