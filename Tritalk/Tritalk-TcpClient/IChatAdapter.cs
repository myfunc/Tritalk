using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Core;
using Tritalk.Libs;

namespace Tritalk.Client
{
    interface ITraceAdapter
    {
        event EventHandler<AcceptTcpDataEventArgs> DataAvailable;
        void SendMessage(Trace trace);
        void StartAdapter();
        void StopAdapter();
    }
}
