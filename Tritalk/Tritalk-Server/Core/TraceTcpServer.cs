using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tritalk.Libs;

namespace Tritalk.Core
{
    /*
     Сервер обрабатывающий Trace-пакеты.
     Конструктор принимает порт и объект обработчика запросов
     */
    public class TraceTcpServer
    {
        private TcpListener m_listener;
        private TraceTcpClientManager clients;

        ITraceHandler i_trace_handler;

        public event EventHandler<AcceptTcpDataEventArgs> AcceptedClient;

        public TraceTcpServer(int port, ITraceHandler traceHandler)
        {
            i_trace_handler = traceHandler;
            InitFields();
            InitTcpServer(port);
        }

        private void OnAcceptClient(TcpClient client, Trace trace)
        {
            EventHandler<AcceptTcpDataEventArgs> ev = Volatile.Read(ref AcceptedClient);
            ev?.Invoke(this, new AcceptTcpDataEventArgs() { Client = client, Trace = trace });
        }

        // Инициализирует поля
        private void InitFields()
        {
            clients = new TraceTcpClientManager();
            ServerActive = false;
        }

        // Инициализирует сокет для прослушивания
        private void InitTcpServer(int port)
        {
            m_listener = new TcpListener(IPAddress.Any, port);
        }
        private bool ServerActive { get; set; }

        public void StopListener()
        {
            ServerActive = false;
            foreach(var i in clients)
            {
                DisconnectClient(i);
            }
        }

        public void SendAll(Trace trace)
        {
            foreach (var i in clients)
            {
                AnswerClient(i.Client, trace);
            }
        }

        public void DisconnectClient(TcpClient client)
        {
            clients.Remove(client);
        }
        public void DisconnectClient(Task client_task)
        {
            clients.Remove(client_task);
        }
        public void DisconnectClient(ActiveClient client)
        {
            clients.Remove(client);
        }

        // Запуск сервера. Прослушивание выполняется асинхронно
        public void StartListener()
        {
            try
            {
                Task.Factory.StartNew(WaitAndAddClients);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.Source);
            }
        }

        private void WaitAndAddClients()
        {
            m_listener.Start();
            ServerActive = true;
            while (ServerActive)
            {
                TcpClient client = m_listener.AcceptTcpClient();

                Console.WriteLine("Client connected: {0}",client.Client.RemoteEndPoint.ToString());

                Task task = Task.Factory.StartNew(() => ListenThisClient(client));
                clients.Add(new ActiveClient() { Client = client, ActiveTask = task });
            }
        }

        private void ListenThisClient(TcpClient client)
        {
            NetworkStream client_stream = client.GetStream();
            BinaryReader client_breader = new BinaryReader(client_stream);
            while (true)
            {
                if (client_stream.DataAvailable)
                {
                    int size = client_breader.ReadInt32();
                    ProcedureStream(client, size);
                }
                    
            }
        }
       
        private void ProcedureStream(TcpClient client, int message_size)
        {
            Trace answer = Trace.Empty;
            NetworkStream stream = client.GetStream();
            try
            {
                BinaryReader br = new BinaryReader(stream);
                byte[] b_trace = br.ReadBytes(message_size);
                answer = TraceJsonBuilder.Deserialize(b_trace);
            }
            catch (Exception e)
            {
                answer = GetErrorTrace();
            }
            finally
            {
                AnswerClient(client, HandleTrace(answer));
                OnAcceptClient(client, answer);
            }

        }

        private void AnswerClient(TcpClient client, Trace trace)
        {
            NetworkStream stream = client.GetStream();
            byte[] b_answer = TraceJsonBuilder.SerializeToBytes(trace);
            byte[] b_answer_sized = PackageBuider.AddSizeToByteArray(b_answer);
            stream.Write(b_answer_sized, 0, b_answer_sized.Length);
        }

        private Trace ProcedureReceive(object receive)
        {
            if (!(receive is Trace)) throw new InvalidReceiveObjectException(receive);
            Trace trace_receive = receive as Trace;
            return HandleTrace(trace_receive);
        }

        private Trace HandleTrace(Trace trace)
        {
            return i_trace_handler.HandleTrace(trace);
        }

        private Trace GetErrorTrace()
        {
            return Trace.Empty;
        }
    }
}
