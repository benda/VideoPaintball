using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VideoPaintballClient.Screens;

namespace VideoPaintballClient
{

    static class Game
    {
        /// <summary>
        /// Shows the splash screen and runs the actual game
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            GameEngine engine = null;
            GameConfiguration gameConfiguration = new GameConfiguration();

            try
            {
                using (SplashScreen splashScreen = new SplashScreen())
                {
                    splashScreen.ShowDialog();
                }

                Lobby lobby = null;
                using (GameConfigurationScreen gameConfigurationScreen = new GameConfigurationScreen(gameConfiguration))
                {
                    gameConfigurationScreen.ShowDialog();
                    lobby = new Lobby(gameConfigurationScreen.GameConfiguration);
                }

                using (LobbyScreen lobbyScreen = new LobbyScreen(lobby))
                {
                    lobbyScreen.ShowDialog();
                }

                engine = new GameEngine(gameConfiguration.ServerConnection);
                Application.Run(engine);
            }
            finally
            {
                if (gameConfiguration != null && gameConfiguration.ServerConnection != null)
                {
                    gameConfiguration.ServerConnection.GetStream().Close();
                    gameConfiguration.ServerConnection.Close();
                }

                if (engine != null)
                {
                    engine.Dispose();
                }
            }
        }
    }
}
