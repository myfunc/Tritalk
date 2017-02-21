using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tritalk.Core;
using Tritalk.Libs;

namespace Tritalk.Log
{
    class LogTraceHandler : ITraceHandler
    {
        ITraceHandler t_handler;

        public event EventHandler<DataRequestEventArgs> Action;
        private void OnAction(Trace trace)
        {
            EventHandler<DataRequestEventArgs> ev = Volatile.Read(ref Action);
            ev?.Invoke(this, new DataRequestEventArgs() { Data = trace });
        }

        public LogTraceHandler(ITraceHandler handler)
        {
            t_handler = handler;
        }

        public event EventHandler<DataRequestEventArgs> DataRequest;

        public Trace HandleTrace(Trace trace)
        {
            OnAction(trace);
            Trace answ = t_handler.HandleTrace(trace);
            OnAction(answ);
            return answ;
        }
    }
}
