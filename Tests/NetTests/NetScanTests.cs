using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Globalization;

using NUnit.Framework;
using VideoPaintballClient.Net.NetScanning;

namespace VideoPaintballTests.NetTests
{
   [TestFixture]
   public class NetScanTests
    {
       [SetUp]
       protected void SetUp()
       {

       }

       [Test]
       public void FindServersTest()
       {
           //This code should work fine - however it causes NUnit to have fatal errors.

           /*
           IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName())[0];
           string start = string.Empty;
           string end = string.Empty;
           if (ip != null)
           {
               byte[] range = ip.GetAddressBytes();
               range[3] = 1;
               start = (new IPAddress(range)).ToString();
               range[3] = 254;
               end = (new IPAddress(range)).ToString();
           }

           IPAddress startIP = null;
           try
           {
               startIP = IPAddress.Parse(start);
           }
           catch (FormatException)
           {
               Console.WriteLine("Start address is not valid.");
               return;
           }

           IPAddress endIP = null;
           try
           {
               endIP = IPAddress.Parse(end);
           }
           catch (FormatException)
           {
               Console.WriteLine("End address is not valid.");
               return;
           }

           Console.WriteLine("start IP: " + startIP.ToString());
           Console.WriteLine("end IP: " + endIP.ToString());


           NetScan ns = new NetScan();
          
           ns.PingComplete += delegate(object s, NetScanPingCompletedEventArgs ev)
           {
               if (ev.Reply.Status == IPStatus.Success)
               {
                   Console.WriteLine("Ping returned: " + ev.Reply.Address.ToString() + " " + ev.Reply.RoundtripTime.ToString(CultureInfo.InvariantCulture) + " ms");
               }
           };

           
           ns.NetScanComplete += delegate(object s, EventArgs ev)
           {
               Console.WriteLine("Scanning complete.");
           };

           Console.WriteLine("Starting Scan...");
           ns.Start(new PingRange(startIP, endIP));
           */
       }

    }
}

           