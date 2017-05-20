using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using VideoPaintballServer;

using NUnit.Framework;

using VideoPaintballCommon.Net;

namespace VideoPaintballTests.NetTests
{
   [TestFixture]
   public class NetworkCommunicatorTests
    {
       private NetworkCommunicator _networkCommunicator;
       private string _sendTestData = "Test Send Data";
       private string _receiveTestData = "Test Receive Data";
       private string _dataServerReceived = string.Empty;
       private string _serverAction = string.Empty;
       private TcpListener _listener;
       private TcpClient _client;

       [SetUp]
       protected void SetUp()
       {
           CreateNetworkCommunicatorAndStartServer();
       }

       [TearDown]
       protected void TearDown()
       {
           _listener.Stop();
           _client.Client.Close();
       }

       [Test]
       public void SendDataTest()
       {
           _serverAction = "Receive";
           _networkCommunicator.SendData( _sendTestData);
           Thread.Sleep(5000); //let server receive data
           Assert.AreEqual(_sendTestData, _dataServerReceived);
      
       }

       [Test]
       public void ReceiveDataTest()
       {
           _serverAction = "Send";
           string data = _networkCommunicator.ReceiveData();
           Assert.AreEqual(_receiveTestData, data);
       }

       private void CreateNetworkCommunicatorAndStartServer()
       {
           Thread serverThread = new Thread(new ThreadStart(Server));
           serverThread.IsBackground = true;
           serverThread.Start();
           Thread.Sleep(5000); //allow server to get set up 
       
           TcpClient networkConnection = new TcpClient();
           networkConnection.Connect(IPUtil.GetLocalIpAddress(), IPUtil.DefaultPort);

           _networkCommunicator = new NetworkCommunicator(networkConnection);
       }

       private void Server()
       {
           _listener = new TcpListener(IPUtil.GetLocalIpAddress(), IPUtil.DefaultPort);
          _listener.Start();
           _client =  _listener.AcceptTcpClient();
           NetworkCommunicator networkCommunicator = new NetworkCommunicator(_client);
         
           Console.WriteLine("Client connected to server successfully.");
           if (_serverAction == "Receive")
           {
               _dataServerReceived = networkCommunicator.ReceiveData();
           }
           else if (_serverAction == "Send")
           {
               networkCommunicator.SendData(_receiveTestData);
           }       
        }
    }
}
