using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace VideoPaintballCommon.Net
{
    public class IPUtil
    {
        public const int DefaultPort = 311;

        public static IPAddress GetLocalIpAddress()
        {
            return IPAddress.Parse("127.0.0.1"); //TODO: using loopback fixes sync issues when going over LAN, loopback shouldn't be necessary

            IPAddress address = null;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties properties = ni.GetIPProperties();

                foreach (IPAddressInformation ai in properties.UnicastAddresses)
                {
                    if (ai.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(ai.Address))
                        continue;

                    address = ai.Address;
                    break;
                }
            }

            return address;
        }
    }
}
