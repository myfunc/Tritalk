using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Tritalk.Libs
{
    public static class TraceJsonBuilder
    {

        public static byte[] SerializeToBytes(Trace trace)
        {
            string json = Serialize(trace);
            return Encoding.Unicode.GetBytes(json);
        }
        public static Trace Deserialize(byte[] trace_bytes)
        {
            string json = Encoding.Unicode.GetString(trace_bytes);
            return Deserialize(json);
        }
        public static string Serialize(Trace trace)
        {
            return JsonConvert.SerializeObject(trace);
        }
        public static Trace Deserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Trace>(json); ;
            }
            catch
            {
                return Trace.Empty;
            }
        }
    }
}
