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
using System.Threading.Tasks;

namespace VideoPaintballServer
{
    class Game
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Game));

        public event EventHandler Started;

        private ManualResetEvent _gameStateProcessed = new ManualResetEvent(false);

        private List<ManualResetEvent> _playersThatSentDataThisTurnList = new List<ManualResetEvent>();
        private ManualResetEvent[] _playersThatSentDataThisTurnArray;

        private List<ManualResetEvent> _endOfTurnWaitList = new List<ManualResetEvent>();
        private ManualResetEvent[] _endOfTurnWaitArray;

        private List<PlayerAction> _currentTurnClientActions = new List<PlayerAction>();
        private Map _completeGameState = new Map();
        private List<TcpClient> _players = new List<TcpClient>();

        private void RunGame()
        {
            OnStarted();

            Task.Factory.StartNew(() =>
            {
                ManualResetEvent endOfTurnWait = new ManualResetEvent(false);

                _endOfTurnWaitList.Add(endOfTurnWait);

                _playersThatSentDataThisTurnArray = _playersThatSentDataThisTurnList.ToArray();
                _endOfTurnWaitArray = _endOfTurnWaitList.ToArray();

                //main game loop
                while (true) //issue [B.2.3] of the design document
                {
                    ManualResetEvent.WaitAll(_playersThatSentDataThisTurnArray); //issue [B.2.3] of the design document

                    _completeGameState.Update(_currentTurnClientActions); //issue [B.2.8] of the design document
                    foreach (ManualResetEvent ev in _endOfTurnWaitArray)
                    {
                        ev.Reset();
                    }
                    _gameStateProcessed.Set();


                    foreach (ManualResetEvent ev in _playersThatSentDataThisTurnArray)
                    {
                        ev.Reset();
                    }

                    endOfTurnWait.Set();
                    ManualResetEvent.WaitAll(_endOfTurnWaitArray); //issue [B.2.3] of the design document
                }

                _gameStateProcessed.Close();
                endOfTurnWait.Close();
            });
        }

        public void PlayerJoined(TcpClient client, string thisClientIP)
        {
            _players.Add(client);

            Player player = new Player(1, thisClientIP);
            _completeGameState.Players.Add(thisClientIP, player);

            NetworkCommunicator.SendData(_players.Count.ToString(), client); //= player ID

            //send this new player to all players player list in game lobby
            foreach (TcpClient playerClient in _players)
            {
                string message = "PlayerJoined:" + thisClientIP;
                NetworkCommunicator.SendData(message, playerClient);

                if (playerClient.Client.RemoteEndPoint.ToString() == thisClientIP)
                {
                    //send new player all existing players
                    foreach (TcpClient playerClientB in _players)
                    {
                        if (playerClientB.Client.RemoteEndPoint.ToString() != thisClientIP)
                        {
                            message = "PlayerJoined:" + playerClientB.Client.RemoteEndPoint.ToString();
                            NetworkCommunicator.SendData(message, playerClient);
                        }
                    }
                }
            }

            Thread playerThread = new Thread(ClientGameLoop);
            playerThread.IsBackground = true;
            playerThread.Start(client);
        }

        private void ClientGameLoop(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            ManualResetEvent sentDataThisTurn = new ManualResetEvent(false);
            ManualResetEvent endOfTurnWait = new ManualResetEvent(false);

            try
            {
                string data = string.Empty;
                string thisClientIP = client.Client.RemoteEndPoint.ToString();
                NetworkCommunicator networkCommunicator = new NetworkCommunicator(client);

                _playersThatSentDataThisTurnList.Add(sentDataThisTurn);
                _endOfTurnWaitList.Add(endOfTurnWait);
                _currentTurnClientActions.Add(new PlayerAction(thisClientIP, MessageConstants.PlayerActionNone));

                _log.InfoFormat("Client {0} connected running on thread: {1}, waiting for start game / 1st turn data", thisClientIP, Thread.CurrentThread.ManagedThreadId);
                data = networkCommunicator.ReceiveData();
                bool readFromClient = false;

                if (data == MessageConstants.StartGame)
                {
                    readFromClient = true;
                    RunGame();
                    foreach (TcpClient clientPlayer in _players)
                    {
                        NetworkCommunicator.SendData(MessageConstants.GameStarting, clientPlayer);
                    }
                    _completeGameState.StartNewGame(_currentTurnClientActions);
                }

                // Loop to receive all the data sent by the client.
                // this is now the main game loop for the game - 1 thread per client
                bool gameIsOn = true;
                while (gameIsOn) //issue [B.2.3] of the design document
                {
                    if (readFromClient)
                    {
                        data = networkCommunicator.ReceiveData();
                    }
                    else
                    {
                        readFromClient = true;
                    }

                    foreach (PlayerAction action in _currentTurnClientActions)
                    {
                        if (action.PlayerID == thisClientIP)
                        {
                            action.Action = data;
                        }
                    }
                    sentDataThisTurn.Set();

                    _gameStateProcessed.WaitOne();

                    networkCommunicator.SendData(_completeGameState.ToString());
                    endOfTurnWait.Set();
                    ManualResetEvent.WaitAll(_endOfTurnWaitArray); //issue [B.2.3] of the design document
                    _gameStateProcessed.Reset();
                }
            }
            catch(Exception ex)
            {
                ErrorUtil.WriteError(ex);
                throw;
            }
            finally
            {
                sentDataThisTurn.Close();
                endOfTurnWait.Close();
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        private void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }
    }
}
