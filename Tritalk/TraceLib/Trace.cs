using System;
using System.Collections.Generic;
using System.Text;

namespace Tritalk.Libs
{
    [Serializable]
    public class Trace : IComparable<Trace> , ICloneable
    {
        private string package_id;
        private string method;
        private string parameters;

        public Trace()
        {
            package_id = Guid.NewGuid().ToString();
        }

        public string PackageId
        {
            get { return package_id; }
            set { package_id = value; }
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
                return new Trace() { Method = "", Parameters = "" };
            }
        }

        public Trace ToError()
        {
            Method = "Error";
            Parameters = "";
            return this;
        }

        public int CompareTo(Trace obj)
        {
            if (PackageId != obj.PackageId) return -1;
            if (Method != obj.Method) return -1;
            if (Parameters != obj.Parameters) return -1;
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Trace)) return false;
            return CompareTo(obj as Trace) == 0;
        }

        public override int GetHashCode()
        {
            return method.GetHashCode() ^ parameters.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Method: {0}; Parameters: {1}", Method, Parameters);
        }

        public Trace GetAnswer(string parameters)
        {
            Trace trace = CloneTrace();
            trace.Parameters = parameters;
            return trace;
        }

        public object Clone()
        {
            return new Trace() { PackageId = this.PackageId, Method = this.Method, Parameters = this.Parameters };
        }

        public Trace CloneTrace()
        {
            return Clone() as Trace;
        }
    }
}