using log4net;
using System;
using System.Net.Sockets;
using System.Threading;
using VideoPaintballCommon.Net;
using VideoPaintballCommon.VPP;
using VideoPaintballServer.Util;

namespace VideoPaintballServer
{
    class ClientListener
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ClientListener));

        public event EventHandler<PlayerJoinedEventArgs> PlayerJoined;

        private bool _listen = true;

        public void Listen()
        {
            TcpListener listener = null;
            TcpClient client = null;

            try
            {
                int port = IPUtil.DefaultPort;
                listener = new TcpListener(IPUtil.GetLocalIpAddress(), port);
                listener.Start();

                _log.InfoFormat("Waiting for a connection on port: {0} ", port);

                while (_listen)
                {
                    if (listener.Pending())
                    {
                        client = listener.AcceptTcpClient();
                        NetworkCommunicator networkCommunicator = new NetworkCommunicator(client);
                        _log.InfoFormat("Client {0} connected.", networkCommunicator.RemoteEndPoint);

                        //acknowledge client is able to join this game
                        networkCommunicator.SendData(MessageConstants.ServerAvailable);

                        string data = networkCommunicator.ReceiveData();
                        if (data == MessageConstants.CloseConnection)
                        {
                            networkCommunicator.Dispose();
                            _log.InfoFormat("Closed client connection - client was just finding servers.");
                        }
                        else if (data == MessageConstants.JoinGame)
                        {
                            OnPlayerJoined(networkCommunicator);                       
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch(Exception ex)
            {
                ErrorUtil.WriteError(ex);
                throw;
            }
            finally
            {
                listener.Stop();
            }
        }

        internal void StopListening()
        {
            _listen = false;
        }

        private void OnPlayerJoined(NetworkCommunicator client)
        {
            PlayerJoined?.Invoke(this, new PlayerJoinedEventArgs(client));
        }
    }
}

