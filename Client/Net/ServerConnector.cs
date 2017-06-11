using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using VideoPaintballCommon.VPP;
using VideoPaintballCommon.Net;
using log4net;

namespace VideoPaintballClient.Net
{
    static class ServerConnector
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Game));

        public static NetworkCommunicator ConnectToServer(IPAddress serverAddress)
        {
            _log.InfoFormat("Connecting to server: [{0}]", serverAddress);

            TcpClient serverConnection = null;
            try
            {
                serverConnection = new TcpClient();
                serverConnection.Connect(new IPEndPoint(serverAddress, IPUtil.DefaultPort));
            }
            catch (SocketException ex)
            {
                _log.Error("Error connecting", ex);

                if (ex.Message == "No connection could be made because the target machine actively refused it")
                {
                    serverConnection = null;
                }
                else if (ex.Message == "A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond")
                {
                    serverConnection = null;
                }
                else if (ex.Message == "An existing connection was forcibly closed by the remote host")
                {
                    serverConnection = null;
                }
                else
                {
                    throw;
                }                
            }

            return new NetworkCommunicator(serverConnection);
        }

        public static bool ServerIsHostingGame(IPAddress serverAddress)
        {
            string data = string.Empty;

            using (NetworkCommunicator client = ConnectToServer(serverAddress))
            { 
                if (client != null)
                {
                    data = client.ReceiveData();
                    client.SendData(MessageConstants.CloseConnection + MessageConstants.MessageEndDelimiter);
                }
            }

            return data == (MessageConstants.ServerAvailable + MessageConstants.MessageEndDelimiter);
        }
    }
}
