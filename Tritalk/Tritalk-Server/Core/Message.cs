using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Core
{
    [Serializable]
    public class Message
    {
        public string Text { get; set; }
        public User Sender { get; set; }
    }
}
