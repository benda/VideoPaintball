using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using VideoPaintballClient.Net;
using VideoPaintballClient.Util;
using VideoPaintballCommon;
using VideoPaintballCommon.MapObjects;
using VideoPaintballCommon.Net;

namespace VideoPaintballClient
{
    public class Game
    {
        private Map _map = null;
        private ServerCommunicator _serverCommunicator;
        public string PlayerAction { get; set; }
        public int TurnsPerSecond { get; private set; }

        Stopwatch stopWatch = Stopwatch.StartNew();
        readonly TimeSpan TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
        readonly TimeSpan MaxElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 10);
        TimeSpan accumulatedTime;
        TimeSpan lastTime;

        public event EventHandler InvalidateNeeded;

        public Game(NetworkCommunicator nc)
        {
            this.ServerCommunications = new ServerCommunicator(nc);
        }

        public void Tick(object sender, EventArgs e)
        {
            TimeSpan currentTime = stopWatch.Elapsed;
            TimeSpan elapsedTime = currentTime - lastTime;
            lastTime = currentTime;

            if (elapsedTime > MaxElapsedTime)
            {
                elapsedTime = MaxElapsedTime;
            }

            accumulatedTime += elapsedTime;

            bool updated = false;

            while (accumulatedTime >= TargetElapsedTime)
            {
                TurnsPerSecond = FrameUtil.CalculateRatePerSecond();

                TakeTurn();

                accumulatedTime -= TargetElapsedTime;
                updated = true;
            }

            if (updated)
            {
                Thread.Sleep(10);

                InvalidateNeeded?.Invoke(this, EventArgs.Empty);
            }
        }

        private void TakeTurn()
        {
            ServerCommunications.SendTurnData(PlayerAction);
            PlayerAction = null;
            MainMap = MapParser.ParseMap(ServerCommunications.ReceiveGameState());
        }

        public void Render(Graphics g)
        {
            if (MainMap != null)
            {
                MainMap.Render(g);
            }
        }

        public Map MainMap
        {
            get { return _map; }
            set { _map = value; }
        }

        public ServerCommunicator ServerCommunications
        {
            get { return _serverCommunicator; }
            set { _serverCommunicator = value; }
        }
    }
}
