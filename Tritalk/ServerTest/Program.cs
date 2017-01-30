using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Client;
using Tritalk.Libs;
using Tritalk.Core;

namespace ServerTest
{
    class Program
    {
        static Trace MakeTrace(string text)
        {
            return new Trace() { ID = "0000", Method = "SendMessage", Parameters = text };
        }

        static void Main(string[] args)
        {
            Console.WriteLine("TcpClient test");
            TraceClient client = new TraceClient();
            client.DataAvailable += OnAccept;
            client.Connect("localhost", 7777);
            client.StartAdapter();
            while (true)
            {
                string mess = Console.ReadLine();
                client.SendMessage(MakeTrace(mess));
            }
        }

        static void OnAccept(object sender, AcceptTcpDataEventArgs args)
        {
            Console.WriteLine(args.Trace);
        }
    }
}
