using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using VideoPaintballServer.Util;

namespace VideoPaintballServer
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Server server = new Server();
            server.Run();

            _log.Info("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
