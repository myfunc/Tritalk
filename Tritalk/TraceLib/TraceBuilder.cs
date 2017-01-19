using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Tritalk.Libs
{
    public static class TraceBuilder
    {
        static BinaryFormatter m_serializer = new BinaryFormatter();

        public static byte[] Serialize(Trace trace)
        {
            MemoryStream mem = new MemoryStream(1024 * 16);
            m_serializer.Serialize(mem, trace);
            return mem.ToArray();
        }

        public static Trace Deserialize(byte[] trace_bytes)
        {
            try
            {
                MemoryStream mem = new MemoryStream(trace_bytes);
                Trace trace = m_serializer.Deserialize(mem) as Trace;
                return trace;
            } catch 
            {
                return Trace.Empty;
            }
        }
    }
}
