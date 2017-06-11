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
using log4net;

namespace VideoPaintballServer
{
    public class Server
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Server));

        private ClientListener _clientListener = new ClientListener();
        private Game _game;

        public void Run()
        {
            _log.Info("Server is starting...");

            _game = new Game();
            _game.Started += _game_Started;

            _clientListener.PlayerJoined += _clientListener_PlayerJoined;
            _clientListener.Listen();
        }

        private void _game_Started(object sender, EventArgs e)
        {
            _clientListener.StopListening();
        }

        private void _clientListener_PlayerJoined(object sender, PlayerJoinedEventArgs e)
        {
            _game.PlayerJoined(e.Client, e.ClientIP);
        }
    }
}
