using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Libs;

namespace Tritalk.Core
{
    public class AcceptDataEventArgs : EventArgs
    {
        public Trace Trace { get; set; }
        public Socket Client { get; set; }
    }

    public class AcceptTcpDataEventArgs : EventArgs
    {
        public Trace Trace { get; set; }
        public TcpClient Client { get; set; }
    }
}
