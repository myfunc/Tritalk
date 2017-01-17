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
        public object Properties { get; set; }

        public static Trace Empty {
            get
            {
                return new Trace() { ID = "", Method = "", Properties = "" };
            }
        }
    }

    [Serializable]
    class TraceUserData
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}

// var a = new Trace();
// a.Method = "Authorization";
// a.Properties = new TraceUserData() { Login = "Vlad", Password = "pass"};