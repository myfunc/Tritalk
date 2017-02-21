using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Libs;

namespace Tritalk.Core
{
    public class DataRequestEventArgs : EventArgs
    {
        public Trace Data { get; set; }
    }
}
