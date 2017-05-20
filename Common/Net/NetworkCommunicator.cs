using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

using VideoPaintballCommon.VPP;

namespace VideoPaintballCommon.Net
{
    /// <summary>
    /// Issue [B.3.2] of the design document
    /// </summary>
    public class NetworkCommunicator
    {
        private TcpClient _networkConnection;
        private StringBuilder _readBuffer = new StringBuilder();

        public NetworkCommunicator(TcpClient networkConnection)
        {
            if (networkConnection == null) throw new ArgumentNullException("networkConnection");

            this._networkConnection = networkConnection;
        }

        public static void SendData(string data, NetworkStream stream)
        {
            if (data[data.Length - 1].ToString() != MessageConstants.MessageEndDelimiter)
            {
                data += MessageConstants.MessageEndDelimiter;
            }

            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
        }

        public void SendData(string data)
        {
            NetworkCommunicator.SendData(data, this.NetworkConnection.GetStream());
        }

        public string ReceiveData()
        {
            string data = string.Empty;
            NetworkStream stream = this.NetworkConnection.GetStream();

            if (stream.CanRead && !_readBuffer.ToString().Contains(MessageConstants.MessageEndDelimiter)) // issue [A.2.5] of the design document
            {
                byte[] buffer = new byte[1024];
                int numberOfBytesRead = 0;

                while (stream.CanRead && !_readBuffer.ToString().Contains(MessageConstants.MessageEndDelimiter)) // issue [A.2.5] of the design document
                {
                    numberOfBytesRead = stream.Read(buffer, 0, buffer.Length);
                    _readBuffer.Append(Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));
                }

                data = ExtractNextMessage(); // issue [A.2.5] of the design document
            }
            else
            {
                data = ExtractNextMessage(); // issue [A.2.5] of the design document
            }

            return data;
        }
   
        /// <summary>
        /// issue [A.2.5] of the design document
        /// </summary>
        /// <returns></returns>
        private string ExtractNextMessage()
        {
            string nextMessage = string.Empty;
            string buffer = _readBuffer.ToString();
            int end = buffer.IndexOf(MessageConstants.MessageEndDelimiter);
            if (end != -1)
            {
                _readBuffer.Remove(0, end + 1);
                return buffer.Substring(0, end);
            }
            else
            {
                return string.Empty;
            }
        }

        private TcpClient NetworkConnection
        {
            get { return _networkConnection; }
        }         
    }
}
