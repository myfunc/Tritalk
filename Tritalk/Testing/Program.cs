using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Libs;
using Tritalk.Core;

namespace Tritalk.Testing
{
    class TestTraceHandler : ITraceHandler
    {
        public Trace HandleTrace(Trace trace)
        {
            Console.WriteLine(trace);
            trace.Parameters = string.Format("Your message is: {0}", trace.Parameters);
            return trace;
        }
    }

    class Program
    {
        static Trace MakeTrace(string text)
        {
            return new Trace() { ID = "0000", Method = "SendMessage", Parameters = text };
        }

        static void Main(string[] args)
        {
            Console.WriteLine("TcpServer test");
            TestTraceHandler handler = new TestTraceHandler();
            TraceTcpServer server = new TraceTcpServer(7777, handler);

            server.StartListener();
            Console.ReadLine();
        }
    }
}
