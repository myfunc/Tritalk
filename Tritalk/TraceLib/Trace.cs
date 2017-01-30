using System;
using System.Collections.Generic;
using System.Text;

namespace Tritalk.Libs
{
    [Serializable]
    public class Trace : IComparable<Trace>
    {
        private string id;
        private string method;
        private string parameters;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public static Trace Empty {
            get
            {
                return new Trace() { ID = "", Method = "", Parameters = "" };
            }
        }

        public int CompareTo(Trace obj)
        {
            if (ID != obj.ID) return -1;
            if (Method != obj.Method) return -1;
            if (Parameters != obj.Parameters) return -1;
            return 0;
        }

        public override string ToString()
        {
            return string.Format("ID: {0}; Method: {1}; Parameters: {2}", ID, Method, Parameters);
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
// a.Parameters = new TraceUserData() { Login = "Vlad", Password = "pass"};