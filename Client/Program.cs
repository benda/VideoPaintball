using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoPaintballClient
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Shows the splash screen and runs the actual game
        /// </summary>
        [STAThread]
        static void Main()
        {
            XmlConfigurator.Configure();

            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            using (GameRunner game = new GameRunner())
            {
                game.Run();
            }
        }
    }
}
