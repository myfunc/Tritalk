using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Core.ChatProtocolObjects;
using Tritalk.Libs;

namespace Tritalk.Client
{
    public class ReceivedDataEventArgs : EventArgs
    {
        public Trace Trace { get; set; }
    }

    public class ProtocolMessageEventArgs : EventArgs
    {
        public ProtocolObject Data { get; set; }
    }
}
