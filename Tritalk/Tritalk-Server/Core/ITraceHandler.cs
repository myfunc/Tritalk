using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Libs;

namespace Tritalk.Core
{
    public interface ITraceHandler
    {
        event EventHandler<DataRequestEventArgs> DataRequest;
        Trace HandleTrace(Trace trace);
    }
}
