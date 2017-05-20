using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using VideoPaintballCommon.Net;
using VideoPaintballCommon.VPP;

namespace VideoPaintballClient.Net
{
    public class ServerCommunicator 
    {
        private NetworkCommunicator _networkCommunicator;

        public ServerCommunicator(NetworkCommunicator networkCommunicator)
        {
            this.NetworkCommunications = networkCommunicator;
        }

        public void SendTurnData(string playerAction)
        {
            if (playerAction == null)
            {
                playerAction = MessageConstants.PlayerActionNone;
            }
           NetworkCommunications.SendData( playerAction );
        }

        public string ReceiveGameState()
        {
           return NetworkCommunications.ReceiveData();
        }

        public NetworkCommunicator NetworkCommunications
        {
            get { return _networkCommunicator; }
            set { _networkCommunicator = value; }
        }
    }
}
