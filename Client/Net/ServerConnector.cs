using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

using VideoPaintballCommon.VPP;
using VideoPaintballCommon.Net;

namespace VideoPaintballClient.Net
{
    static class ServerConnector
    {
        public static TcpClient ConnectToServer(string serverAddress)
        {
            TcpClient serverConnection = null;
            try
            {
                serverConnection = new TcpClient();
                serverConnection.Connect(new IPEndPoint(IPAddress.Parse(serverAddress), IPUtil.DefaultPort));
            }
            catch (SocketException ex)
            {
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
                    //throw;
                }                
            }

            return serverConnection;
        }

        public static bool ServerIsHostingGame(IPAddress ip)
        {
            TcpClient client = null;
            string data = string.Empty;

            try
            {
                client = ServerConnector.ConnectToServer(ip.ToString());
                if (client != null)
                {
                    if (client.GetStream().CanRead)
                    {
                        int numberOfBytesRead = 0;
                        byte[] buffer = new byte[1024];
                        numberOfBytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
                        data = Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead);

                        buffer = System.Text.Encoding.ASCII.GetBytes( MessageConstants.CloseConnection + MessageConstants.MessageEndDelimiter );
                        client.GetStream().Write(buffer, 0, buffer.Length);      
                    }
                }
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }

            return data == (MessageConstants.ServerAvailable + MessageConstants.MessageEndDelimiter);
        }
    }
}
