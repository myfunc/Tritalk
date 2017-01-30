using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tritalk.Libs;
using Tritalk.Core;
using System.Threading;
using System.IO;

namespace Tritalk.Client
{
    public class TraceClient : ITraceAdapter, IDisposable
    {
        TcpClient connection = null;
        Task listen_task = null;
        public event EventHandler<AcceptTcpDataEventArgs> DataAvailable;

        private void OnDataAvailable(Trace trace)
        {
            EventHandler<AcceptTcpDataEventArgs> ev = Volatile.Read(ref DataAvailable);
            ev?.Invoke(this, new AcceptTcpDataEventArgs() { Client = connection, Trace = trace });
        }

        public TraceClient()
        {
            connection = new TcpClient();
        }

        public void Connect(string uri, int port)
        {
            connection.Connect(uri, port);
        }

        public void StartAdapter()
        {
            listen_task = ListenServerAsync();
        }

        public void StopAdapter()
        {
            if (listen_task != null)
            {
                listen_task.Dispose();
                listen_task = null;
            }
        }

        /* public async Task SendRequestAsync(Trace trace)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    NetworkStream stream = connection.GetStream();
                    byte[] btrace = TraceJsonBuilder.Serialize(trace);
                    byte[] btrace_sized = PackageBuider.AddSizeToByteArray(btrace);
                    stream.Write(btrace_sized, 0, btrace_sized.Length);
                }
            );
        }
        */

        public void SendMessage(Trace trace)
        {
            NetworkStream stream = connection.GetStream();
            byte[] btrace = TraceJsonBuilder.SerializeToBytes(trace);
            byte[] btrace_sized = PackageBuider.AddSizeToByteArray(btrace);
            stream.Write(btrace_sized, 0, btrace_sized.Length);
        }

        private async Task ListenServerAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                ListenServer();
            });
        }

        // Бесконечный while. Вызывать только асинхронно.
        private void ListenServer()
        {
            if (connection == null) return;
            while (true)
            {
                if (connection.GetStream().DataAvailable)
                {
                    BinaryReader br = new BinaryReader(connection.GetStream());
                    int size = br.ReadInt32();
                    Trace trace = TraceJsonBuilder.Deserialize(br.ReadBytes(size));
                    OnDataAvailable(trace);
                }
            }
        }

        public void Disconnect()
        {
            StopAdapter();
            connection.Close();
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}