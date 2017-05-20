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
