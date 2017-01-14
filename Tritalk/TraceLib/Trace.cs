using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Libs
{
    [Serializable]
    public class Trace
    {
        public string ID { get; set; }
        public string Method { get; set; }
        public string Properties { get; set; }
    }
}
