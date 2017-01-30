using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tritalk.Core;
using Tritalk.Libs;

namespace Tritalk.Client
{
    class ChatClient
    {
        User user;
        ITraceAdapter traceAdapter = null;

        public ChatClient(ITraceAdapter adapter)
        {
            traceAdapter = adapter;
            Init();
        }

        public ChatClient()
        {
            traceAdapter = new TraceClient();
            Init();
        }

        void Init()
        {
            user = new User();
            traceAdapter.DataAvailable += ClientDataAvailable;
        }

        void ClientDataAvailable(object sender, AcceptTcpDataEventArgs args)
        {
            Trace trace = args.Trace;
            ProcedureTrace(trace);
        }

        void ProcedureTrace(Trace trace)
        {

        }
    }
}
