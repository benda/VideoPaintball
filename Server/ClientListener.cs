using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

using VideoPaintballCommon.Detectors;
using VideoPaintballCommon.Net;
using VideoPaintballCommon.VPP;
using VideoPaintballCommon.MapObjects;
using VideoPaintballCommon.Util;
using VideoPaintballCommon;
using VideoPaintballServer.Util;
using log4net;

namespace VideoPaintballServer
{
    class ClientListener
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ClientListener));

        public event EventHandler<PlayerJoinedEventArgs> PlayerJoined;
        public event EventHandler<PlayerJoinedEventArgs> PlayerStart;
        public event EventHandler GameStart;

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

                //wait for connections loop
                while (_listen)
                {
                    if (listener.Pending())
                    {
                        client = listener.AcceptTcpClient();
                        string thisClientIP = client.Client.RemoteEndPoint.ToString();
                        NetworkCommunicator networkCommunicator = new NetworkCommunicator(client);
                        _log.InfoFormat("Client {0} connected.", thisClientIP);

                        //acknowledge client is able to join this game
                        networkCommunicator.SendData(MessageConstants.ServerAvailable);

                        string data = networkCommunicator.ReceiveData();
                        if (data == MessageConstants.CloseConnection)
                        {
                            client.Client.Close();
                            client.Close();
                            _log.InfoFormat("Closed client connection - client was just finding servers.");
                        }
                        else if (data == MessageConstants.JoinGame)
                        {
                            OnPlayerJoined(thisClientIP, client);                       
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

        private void OnPlayerJoined(string clientIP, TcpClient tcpClient)
        {
            PlayerJoined?.Invoke(this, new PlayerJoinedEventArgs(clientIP, tcpClient));
        }

        private void OnPlayerStart(string clientIP, TcpClient tcpClient)
        {
            PlayerStart?.Invoke(this, new PlayerJoinedEventArgs(clientIP, tcpClient));
        }

        private void OnGameStart()
        {
            GameStart?.Invoke(this, EventArgs.Empty);
        }
    }
}

