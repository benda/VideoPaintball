using System;
using System.Collections.Generic;
using System.Text;


using VideoPaintballServer.Util;

namespace VideoPaintballServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();

            Console.ReadLine();
            Console.WriteLine("Press any key to exit...");
        }
    }
}
