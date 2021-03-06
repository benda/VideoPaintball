﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

using VideoPaintballCommon.Net;
using VideoPaintballCommon.VPP;
using VideoPaintballClient.Net;

namespace VideoPaintballClient
{
    public class Lobby
    {
        private Thread _playerListLoadThread = null;

        public event EventHandler<PlayerJoinedEventArgs> PlayerJoined;
        public event EventHandler GameStarting;

        public Lobby(GameConfiguration configuration)
        {
            GameConfiguration = configuration;
        }

        public void Wait()
        {
            _playerListLoadThread = new Thread(new ThreadStart(WaitInLobby));
            _playerListLoadThread.Name = "LobbyWaitThread";
            _playerListLoadThread.IsBackground = true;
            _playerListLoadThread.Start();
        }

        private void WaitInLobby()
        {
            string data = string.Empty;
            bool continueToLoop = true;
            while (continueToLoop)
            {
                data = GameConfiguration.ServerConnection.ReceiveData();
                if (data.Contains(MessageConstants.PlayerJoined))
                {
                    data = data.Substring(data.IndexOf(":") + 1);
                    OnPlayerJoined(data);
                }
                else if (data.Contains(MessageConstants.GameStarting))
                {
                    continueToLoop = false;
                }
                else if (data == MessageConstants.ServerAvailable)
                {
                    GameConfiguration.ServerConnection.SendData(MessageConstants.JoinGame);
                }

                Thread.Sleep(1000);
            }

            Thread.Sleep(3000);

            OnGameStarting();

            return;
        }

        public void StartGame()
        {
            GameConfiguration.ServerConnection.SendData(MessageConstants.StartGame);
        }

        private delegate void RaiseGameStartingHandler();
        private void OnGameStarting()
        {
            EventHandler handler = GameStarting;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private delegate void RaisePlayerJoinedHandler(string playerIPAddress);
        private void OnPlayerJoined(string playerIPAddress)
        {
            PlayerJoinedEventArgs ev = new PlayerJoinedEventArgs();
            ev.PlayerIPAddress = playerIPAddress;
            PlayerJoined(this, ev);
        }

        public GameConfiguration GameConfiguration { get; private set; }
    }
}
