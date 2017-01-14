using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tritalk.Libs;

namespace Tritalk.Core
{
    class ChatServer
    {
        private Socket m_listener;
        private const int BUFFER_SIZE = 1024 * 16;

        public ChatServer(int port)
        {
            InitListener(port);
        }

        private void InitListener(int port)
        {
            IPAddress ipadress = IPAddress.Any;
            IPEndPoint ipendpoint = new IPEndPoint(ipadress, port);
            m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_listener.Bind(ipendpoint);
        }

        private void StartListener()
        {
            try
            {
                ListenAndProcedureClientsAsync();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,e.Source);
            }
            finally
            {
                m_listener.Dispose();
            }
        }

        private Task ListenAndProcedureClientsAsync()
        {
            return Task.Factory.StartNew(AcceptAndProcedureClient);
        }

        private void AcceptAndProcedureClient()
        {
            Socket rec = m_listener.Accept();
            MemoryStream mem_stream = new MemoryStream(BUFFER_SIZE);
            rec.Receive(mem_stream.GetBuffer());
            BinaryFormatter serializer = new BinaryFormatter();
            object receive_obj = serializer.Deserialize(mem_stream);
            ProcedureReceive(receive_obj);

        }

        private void ProcedureReceive(object receive)
        {
            if (!(receive is Trace)) throw new InvalidReceiveObjectException(receive);
            Trace trace_receive = receive as Trace;
        }
    }
}
