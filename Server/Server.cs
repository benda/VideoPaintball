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

namespace VideoPaintballServer
{
    public class Server
    {
        private ManualResetEvent gameStateProcessed = new ManualResetEvent(false);

        private List<ManualResetEvent> _playersThatSentDataThisTurnList = new List<ManualResetEvent>();
        private ManualResetEvent[] _playersThatSentDataThisTurnArray;

        private List<ManualResetEvent> _endOfTurnWaitList = new List<ManualResetEvent>();
        private ManualResetEvent[] _endOfTurnWaitArray;

        private bool _sendGameStarting = false;
        private List<TcpClient> _players = new List<TcpClient>();
        private List<PlayerAction> _currentTurnClientActions = new List<PlayerAction>();
        private Map _completeGameState = new Map();

        public void Run()
        {
            Trace.WriteLine("Server is starting...");

            TcpListener server = null;
            TcpClient client = null;
            ManualResetEvent endOfTurnWait = new ManualResetEvent(false);

            try
            {
                int port = IPUtil.DefaultPort;
                // server = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                server = new TcpListener(IPUtil.GetLocalIpAddress(), port);
                server.Start();

                _endOfTurnWaitList.Add(endOfTurnWait);
                Trace.WriteLine(string.Format("Waiting for a connection on port: {0} ", port));

                //wait for connections loop
                while (!_sendGameStarting)
                {
                    if (server.Pending())
                    {
                        client = server.AcceptTcpClient();
                        string thisClientIP = client.Client.RemoteEndPoint.ToString();
                        NetworkCommunicator networkCommunicator = new NetworkCommunicator(client);
                        Trace.WriteLine("Client {0} connected.", thisClientIP);

                        //acknowledge client is able to join this game
                        networkCommunicator.SendData(MessageConstants.ServerAvailable);

                        string data = networkCommunicator.ReceiveData();
                        if (data == MessageConstants.CloseConnection)
                        {
                            //client was just finding all servers
                            client.Client.Close();
                            client.Close();
                            Trace.WriteLine("Closed client connection - client was just finding servers.");
                        }
                        else if (data == MessageConstants.JoinGame)
                        {
                            _players.Add(client);

                            Player player = new Player(1, thisClientIP);
                            _completeGameState.Players.Add(thisClientIP, player);

                            networkCommunicator.SendData(_players.Count.ToString()); //= player ID

                            //send this new player to all players player list in game lobby
                            foreach (TcpClient playerClient in _players)
                            {
                                string message = "PlayerJoined:" + thisClientIP;
                                NetworkCommunicator.SendData(message, playerClient.GetStream());

                                if (playerClient.Client.RemoteEndPoint.ToString() == thisClientIP)
                                {
                                    //send new player all existing players
                                    foreach (TcpClient playerClientB in _players)
                                    {
                                        if (playerClientB.Client.RemoteEndPoint.ToString() != thisClientIP)
                                        {
                                            message = "PlayerJoined:" + playerClientB.Client.RemoteEndPoint.ToString();
                                            NetworkCommunicator.SendData(message, playerClient.GetStream());
                                        }
                                    }
                                }

                            }

                            Thread playerThread = new Thread(ClientGameLoop);
                            playerThread.IsBackground = true;
                            playerThread.Start(client);
                        }
                    }
                    Thread.Sleep(100);
                }

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
                    gameStateProcessed.Set();


                    foreach (ManualResetEvent ev in _playersThatSentDataThisTurnArray)
                    {
                        ev.Reset();
                    }

                    endOfTurnWait.Set();
                    ManualResetEvent.WaitAll(_endOfTurnWaitArray); //issue [B.2.3] of the design document
                }
            }
            finally
            {
                gameStateProcessed.Close();
                endOfTurnWait.Close();
                server.Stop();
                Console.ReadLine();
            }
        }



        private void ClientGameLoop(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            String data = string.Empty;
            string thisClientIP = string.Empty;
            ManualResetEvent sentDataThisTurn = new ManualResetEvent(false);
            ManualResetEvent endOfTurnWait = new ManualResetEvent(false);

            try
            {
                thisClientIP = client.Client.RemoteEndPoint.ToString();
                NetworkCommunicator networkCommunicator = new NetworkCommunicator(client);

                _playersThatSentDataThisTurnList.Add(sentDataThisTurn);
                _endOfTurnWaitList.Add(endOfTurnWait);
                _currentTurnClientActions.Add(new PlayerAction(thisClientIP, MessageConstants.PlayerActionNone));

                Trace.WriteLine(string.Format("Client {0} connected running on thread: {1}", thisClientIP, Thread.CurrentThread.ManagedThreadId));

                Trace.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId.ToString() + " waiting for start game / 1st turn data");
                data = networkCommunicator.ReceiveData();
                Trace.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId.ToString() + " received: " + data);
                bool readFromClient = false;

                if (data == MessageConstants.StartGame)
                {
                    readFromClient = true;
                    _sendGameStarting = true;
                    foreach (TcpClient clientPlayer in _players)
                    {
                        NetworkCommunicator.SendData(MessageConstants.GameStarting, clientPlayer.GetStream());
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

                    gameStateProcessed.WaitOne();

                    networkCommunicator.SendData(_completeGameState.ToString());
                    endOfTurnWait.Set();
                    ManualResetEvent.WaitAll(_endOfTurnWaitArray); //issue [B.2.3] of the design document
                    gameStateProcessed.Reset();
                }
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
    }
}
