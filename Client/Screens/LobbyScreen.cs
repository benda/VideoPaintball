using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VideoPaintballCommon.VPP;


namespace VideoPaintballClient.Screens
{
    public partial class LobbyScreen : Form
    {
        private Lobby _lobby;

        public LobbyScreen(Lobby lobby)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(lobby.GameConfiguration.ServerIPAddress))
            {
                serverLabel.Text += "You are the server";
            }
            else
            {
                serverLabel.Text += lobby.GameConfiguration.ServerIPAddress;
            }

            _lobby = lobby;
            _lobby.PlayerJoined += new EventHandler<PlayerJoinedEventArgs>(LobbyScreen_PlayerJoined);
            _lobby.GameStarting += new EventHandler(LobbyScreen_GameStarting);
        }

        private void LobbyScreen_Load(object sender, System.EventArgs e)
        {
            _lobby.Wait();
        }

        private void startGameButton_Click(object sender, EventArgs e)
        {
            startGameButton.Enabled = startGameButton.Visible = false;
            _lobby.StartGame();
        }

        void LobbyScreen_GameStarting(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                this.Hide();
            });
        }

        void LobbyScreen_PlayerJoined(object sender, PlayerJoinedEventArgs e)
        {
            Invoke((Action)delegate
            {
                playerListBox.Items.Add(e.PlayerIPAddress);
                startGameButton.Enabled = _lobby.GameConfiguration.ThisClientIsServer;
            });
        }
    }
}