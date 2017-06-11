using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

using VideoPaintballCommon.VPP;
using System.Diagnostics;
using log4net;
using System.Net;

namespace VideoPaintballCommon.Net
{
    /// <summary>
    /// Issue [B.3.2] of the design document
    /// </summary>
    public class NetworkCommunicator : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(NetworkCommunicator));

        private TcpClient _networkConnection;
        private StringBuilder _readBuffer = new StringBuilder();

        public NetworkCommunicator(TcpClient networkConnection)
        {
            this._networkConnection = networkConnection ?? throw new ArgumentNullException("networkConnection");
        }

        public void SendData(string data)
        {
            NetworkStream stream = NetworkConnection.GetStream();
            _log.DebugFormat("Sending [{0}] to [{1}]: ", data, RemoteEndPoint);

            if (data[data.Length - 1].ToString() != MessageConstants.MessageEndDelimiter)
            {
                data += MessageConstants.MessageEndDelimiter;
            }

            byte[] buffer = Encoding.ASCII.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);

            _log.Debug("Sent.");            
        }

        public string ReceiveData()
        {
            string data = string.Empty;
            NetworkStream stream = this.NetworkConnection.GetStream();

            _log.DebugFormat("Receiving on [{0}]...", NetworkConnection.Client.LocalEndPoint);

            if (stream.CanRead && !_readBuffer.ToString().Contains(MessageConstants.MessageEndDelimiter)) // issue [A.2.5] of the design document
            {
                byte[] buffer = new byte[1024];
                int numberOfBytesRead = 0;

                while (stream.CanRead && !_readBuffer.ToString().Contains(MessageConstants.MessageEndDelimiter)) // issue [A.2.5] of the design document
                {
                    numberOfBytesRead = stream.Read(buffer, 0, buffer.Length);
                    _readBuffer.Append(Encoding.ASCII.GetString(buffer, 0, numberOfBytesRead));

                    _log.DebugFormat("Read Buffer: [{0}]", _readBuffer.ToString());
                }

                data = ExtractNextMessage(); // issue [A.2.5] of the design document
            }
            else
            {
                data = ExtractNextMessage(); // issue [A.2.5] of the design document
            }

            _log.DebugFormat("Received: [{0}]", data);

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

        public EndPoint RemoteEndPoint
        {
            get { return NetworkConnection.Client.RemoteEndPoint; }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _networkConnection.GetStream().Close();
                    _networkConnection.Close();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
