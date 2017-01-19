using System;
using System.Collections.Generic;
using System.Text;

namespace Tritalk.Libs
{
    [Serializable]
    public class Trace : IComparable<Trace>
    {
        public string ID { get; set; }
        public string Method { get; set; }
        public string Properties { get; set; }

        public static Trace Empty {
            get
            {
                return new Trace() { ID = "", Method = "", Properties = "" };
            }
        }

        public int CompareTo(Trace obj)
        {
            if (ID != obj.ID) return -1;
            if (Method != obj.Method) return -1;
            if (Properties != obj.Properties) return -1;
            return 0;
        }

        public override string ToString()
        {
            return string.Format("ID: {0}; Method: {1}; Properties: {2}", ID, Method, Properties);
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