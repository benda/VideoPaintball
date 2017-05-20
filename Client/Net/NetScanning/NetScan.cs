/*
This namespace classes from msdn.microsoft.com/coding4fun, modified for our use though
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;


namespace VideoPaintballClient.Net.NetScanning
{
    /// <summary>
    /// Note that we raise events here through using Control.Invoke; Control.Invoke
    /// properly calls the event on this thread's creating thread, which should be the UI thread. If this
    /// is not done, the UI thread will not properly update controls.
    /// </summary>
    public class NetScan : Control
    {
        public event EventHandler<NetScanPingCompletedEventArgs> PingComplete;
        public event EventHandler<EventArgs> NetScanComplete;
       
        //
        // Limits the number of pings happening simultaniously
        //
        const int THREAD_COUNT = 200;
        private Semaphore pingBlock = new Semaphore(0, THREAD_COUNT);

        public NetScan()
        {
            //
            // All items in the semaphore start out locked.  This releases them all.
            //
            pingBlock.Release(THREAD_COUNT);

            //needed for Control.Invoke
            IntPtr i = this.Handle;
        }

        /// <summary>
        /// Starts ping operations, running on background thread
        /// </summary>
        /// <param name="pr">Contains the starting and ending IP address for the pings.</param>
        public void Start(PingRange pr)
        {
             Thread t = new Thread(new ParameterizedThreadStart(pingWorker_DoWork));
             t.Start(new PingRange(pr.StartRange, pr.EndRange));
        }

        /// <summary>
        /// Loops through the IP address and does the pings.
        /// </summary>
        /// <param name="o">Start and end IP addresses to ping.</param>
        private void pingWorker_DoWork(object o)
        {
            PingRange pingRange = (PingRange)o;

            //
            // Get the starting and ending address as a byte array to make it easier to
            // loop through.
            //
            byte[] start = pingRange.StartRange.GetAddressBytes();
            byte[] end = pingRange.EndRange.GetAddressBytes();

            LinkedList<Thread> threads = new LinkedList<Thread>();

            //
            // Loop through each octet in the IP address, and ping on a background thread.
            //
            for (byte o0 = start[0]; o0 <= end[0]; o0++)
                for (byte o1 = start[1]; o1 <= end[1]; o1++)
                    for (byte o2 = start[2]; o2 <= end[2]; o2++)
                        for (byte o3 = start[3]; o3 <= end[3]; o3++)
                        {
                            pingBlock.WaitOne();
                            Thread t = new Thread(new ParameterizedThreadStart(DoPing));
                            t.Start(new IPAddress(new byte[] { o0, o1, o2, o3 }));
                            threads.AddLast(t);
                        }

            //
            // Wait until all the pings are done.
            //
            foreach (Thread ln in threads)
            {
                ln.Join();
            }

            //
            // Raise an event saying the pings are done on the main UI thread.
            //
            try
            {
                this.Invoke(new RaiseNetScanCompleteHandler(RaiseNetScanComplete));         
            }
            catch (InvalidOperationException)
            {
                // Can happen if the application is closed while pings are going on in 
                // the background.  Safe to ignore.
            }

        }



        /// <summary>
        /// Perform the actual ping, and release the semaphore so that another thread
        /// can go.
        /// </summary>
        /// <param name="o">IP address to ping</param>
        private void DoPing(object o)
        {
            IPAddress ip = (IPAddress)o;
            Ping ping = new Ping();
            PingReply pr = ping.Send(ip);
            pingBlock.Release();
            try
            {
                bool serverIsHostingGame = false;
                if (pr.Status == IPStatus.Success)
                {
                    if (ServerConnector.ServerIsHostingGame(ip))
                    {
                        serverIsHostingGame = true;
                    }
                }
                this.Invoke(new RaisePingCompleteHandler(RaisePingComplete), new object[] { pr, serverIsHostingGame });
                    
            }
            catch (InvalidOperationException)
            {
                // Can happen if the application is closed while pings are going on in 
                // the background.  Safe to ignore.
            }
        }


        private delegate void RaiseNetScanCompleteHandler();
        private void RaiseNetScanComplete()
        {
            NetScanComplete(this, new EventArgs());
        }



        private delegate void RaisePingCompleteHandler(PingReply pr, bool serverIsHostingGame);
        private void RaisePingComplete(PingReply pr, bool serverIsHostingGame)
        {
            NetScanPingCompletedEventArgs e = new NetScanPingCompletedEventArgs();
            e.Reply = pr;
            e.ServerIsHostingGame = serverIsHostingGame;
            PingComplete(this, e);
        }

      }
}
