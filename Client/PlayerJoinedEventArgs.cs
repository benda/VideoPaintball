using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoPaintballClient
{
    public class PlayerJoinedEventArgs : EventArgs
    {
        private string _playerIPAddress;

        public PlayerJoinedEventArgs()
        {

        }

        public string PlayerIPAddress
        {
            get { return _playerIPAddress; }
            set { _playerIPAddress = value; }
        }
    }
}
