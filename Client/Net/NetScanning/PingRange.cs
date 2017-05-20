using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace VideoPaintballClient.Net.NetScanning
{
    public class PingRange
    {
        private IPAddress _startRange;
        public IPAddress StartRange
        {
            get
            {
                return _startRange;
            }

            set
            {
                _startRange = value;
            }
        }

        private IPAddress _endRange;
        public IPAddress EndRange
        {
            get
            {
                return _endRange;
            }

            set
            {
                _endRange = value;
            }
        }

        public PingRange(IPAddress startRange, IPAddress endRange)
        {
            _startRange = startRange;
            _endRange = endRange;
        }

        public PingRange(long startRange, long endRange)
        {
            _startRange = new IPAddress(startRange);
            _endRange = new IPAddress(endRange);
        }

    }
}
