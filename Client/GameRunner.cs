using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VideoPaintballClient.Screens;

namespace VideoPaintballClient
{
    class GameRunner : IDisposable
    {
        GameScreen _gameScreen = null;
        GameConfiguration _gameConfiguration = new GameConfiguration();

        public void Run()
        {
            using (GameConfigurationScreen gameConfigurationScreen = new GameConfigurationScreen(_gameConfiguration))
            {
                gameConfigurationScreen.ShowDialog();
            }

            Lobby lobby = new Lobby(_gameConfiguration);
            using (LobbyScreen lobbyScreen = new LobbyScreen(lobby))
            {
                lobbyScreen.ShowDialog();
            }

            Game game = new Game(_gameConfiguration.ServerConnection);
            _gameScreen = new GameScreen(game);

            Application.Run(_gameScreen);
        }

        public void Dispose()
        {
            if (_gameConfiguration != null && _gameConfiguration.ServerConnection != null)
            {
                _gameConfiguration.ServerConnection.Dispose();
            }

            if (_gameScreen != null)
            {
                _gameScreen.Dispose();
            }
        }
    }
}
