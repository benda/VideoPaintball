using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoPaintballClient
{
    class Program
    {
        /// <summary>
        /// Shows the splash screen and runs the actual game
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            using (GameRunner game = new GameRunner())
            {
                game.Run();
            }
        }
    }
}
