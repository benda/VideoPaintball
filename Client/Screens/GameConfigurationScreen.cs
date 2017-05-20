using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;

using VideoPaintballClient.Net;
using VideoPaintballClient.Net.NetScanning;
using VideoPaintballCommon.Net;
using System.Threading;

namespace VideoPaintballClient.Screens
{
    public partial class GameConfigurationScreen : Form
    {
        private int _pingsReturned = 0;

        public GameConfigurationScreen(GameConfiguration gameConfiguration)
        {
            InitializeComponent();
            GameConfiguration = gameConfiguration;
        }

        private void findServersButton_Click(object sender, EventArgs e)
        {
            findServersButton.Enabled = false;

            IPAddress ip = IPUtil.GetLocalIpAddress();
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
                MessageBox.Show("Start address is not valid.");
                return;
            }

            IPAddress endIP = null;
            try
            {
                endIP = IPAddress.Parse(end);
            }
            catch (FormatException)
            {
                MessageBox.Show("End address is not valid.");
                return;
            }

            NetScan ns = new NetScan();
            ns.NetScanComplete += new EventHandler<EventArgs>(ns_NetScanComplete);
            ns.PingComplete += new EventHandler<NetScanPingCompletedEventArgs>(ns_PingComplete);

            toolStripStatusLabel1.Text = "Scanning in progress...";
            ns.Start(new PingRange(startIP, endIP));
        }

        private void ns_PingComplete(object sender, NetScanPingCompletedEventArgs ev)
        {
            _pingsReturned++;
            if (ev.Reply.Status == IPStatus.Success)
            {
                string item = ev.Reply.Address.ToString() + " Latency: " + ev.Reply.RoundtripTime.ToString(System.Globalization.CultureInfo.InvariantCulture) + " ms";
                if (ev.ServerIsHostingGame)
                {
                    item += " Status: Available For Game";
                    serversListBox.Items.Add(item.ToString());
                }
            }

            toolStripStatusLabel1.Text = String.Format("Scanning in progress...{0}/254 addresses checked", _pingsReturned.ToString());
        }

        private void ns_NetScanComplete(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Scanning complete.";
            findServersButton.Enabled = true;

            if (serversListBox.Items.Count == 0)
            {
                toolStripStatusLabel1.Text = "Scanning complete. No Servers found!";
            }
            else
            {
                connectToServerButton.Enabled = true;
            }
            _pingsReturned = 0;
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            startGameButton.Enabled = false;

            Process process = new Process();

#if DEBUG
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.FileName = "..\\..\\..\\Server\\bin\\debug\\VideoPaintballServer.exe";
#elif RELEASE
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "..\\..\\..\\Server\\bin\\release\\VideoPaintballServer.exe";
#endif

            process.StartInfo.UseShellExecute = true;
            process.Start();

            Thread.Sleep(5000);

            GameConfiguration.ServerConnection = ServerConnector.ConnectToServer(IPUtil.GetLocalIpAddress().ToString());
            GameConfiguration.ThisClientIsServer = true;

            if (GameConfiguration.ServerConnection != null)
            {
                this.Hide();
            }
            else
            {
                startGameButton.Enabled = true;
            }
        }

        private void connectToServerButton_Click(object sender, EventArgs e)
        {
            if (serversListBox.SelectedIndex > -1)
            {
                string serverAddress = (string)serversListBox.SelectedItem;
                GameConfiguration.ServerIPAddress = serverAddress;
                if (serverAddress.IndexOf("Available", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    serverAddress = serverAddress.Remove(serverAddress.IndexOf(" L", StringComparison.InvariantCultureIgnoreCase));
                    GameConfiguration.ServerConnection = ServerConnector.ConnectToServer(serverAddress);

                    if (GameConfiguration.ServerConnection != null)
                    {
                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("That server is not available for a game at this time.");
                }
            }
        }

        public GameConfiguration GameConfiguration { get; private set; }
    }
}