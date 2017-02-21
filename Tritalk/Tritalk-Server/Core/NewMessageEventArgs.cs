using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Core;

namespace Tritalk.Core
{
    public class NewMessageEventArgs : EventArgs
    {
        public Message Message { get; set; }
    }
}
