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
    class TraceServer
    {
        BinaryFormatter m_serializer;
        CancellationToken m_cancel_token; // TODO: Добавить возможность остановки сервера
        // Слушатель сервера
        private Socket m_listener;
        // Максимальный размер входящего пакета данных
        private const int BUFFER_SIZE = 1024 * 1;

        ITraceHandler i_trace_handler;

        public event EventHandler<AcceptDataEventArgs> AcceptedClient;

        public TraceServer(int port, ITraceHandler traceHandler)
        {
            i_trace_handler = traceHandler;
            InitFields();
            InitListener(port);
        }

        public void OnAcceptClient(Socket client, Trace trace)
        {
            EventHandler<AcceptDataEventArgs> ev = Volatile.Read(ref AcceptedClient);
            ev?.Invoke(this, new AcceptDataEventArgs() { Client = client, Trace = trace });
        }

        // Инициализирует поля
        private void InitFields()
        {
            m_serializer = new BinaryFormatter();
            m_cancel_token = CancellationToken.None;
        }

        // Инициализирует сокет для прослушивания
        private void InitListener(int port)
        {
            IPAddress ipadress = IPAddress.Any;
            IPEndPoint ipendpoint = new IPEndPoint(ipadress, port);
            m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_listener.Bind(ipendpoint);
        }

        public void StopListener()
        {
            if (m_cancel_token == CancellationToken.None) return;
        }
        // Запуск сервера. Прослушивание выполняется асинхронно
        public void StartListener()
        {
            if (m_cancel_token != CancellationToken.None) return;
            try
            {
                ListenAndProcedureClientsAsync();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,e.Source);
            }
        }

        private Task ListenAndProcedureClientsAsync()
        {
            Task listen_task = Task.Factory.StartNew(
                () =>
                {
                    m_listener.Listen(100);
                    while (true)
                    {
                        AcceptAndProcedureAndAnswerClient();
                    };
                }
            );
            return listen_task;
        }

        private Trace VladParse(string traceStr)
        {
            string[] result;
            result = traceStr.Split(',');
            Trace trace = new Trace();

            trace.ID = result[0];
            trace.Method = result[1];
            trace.Properties = result[2];

            return trace;
        }

        // Ожидает клиента, затем считывает Trace-пакет данных, обрабатывает и возвращает ответ.
        private void AcceptAndProcedureAndAnswerClient()
        {
            Socket client = m_listener.Accept();

            try
            {
                /* Мой способ
                object receive_obj = m_serializer.Deserialize(mem_receive);
                Trace answer = ProcedureReceive(receive_obj);
                */
                // Для Влада
                byte[] answer_byte = ReceiveClient(client);
                string vstr = Encoding.UTF8.GetString(answer_byte);
                Trace answer = VladParse(vstr);
                
                AnswerClient(client, HandleTrace(answer));
                OnAcceptClient(client, answer);
            }
            catch (Exception e)
            {
                AnswerError(client);
            }
            
        }

        private byte[] ReceiveClient(Socket client)
        {
            // Мудреный метод, упростить.
            MemoryStream mem_receive = new MemoryStream(BUFFER_SIZE);
            int rec_bytes = client.Receive(mem_receive.GetBuffer());
            byte[] rec_seeked = new byte[rec_bytes];
            for (int i = 0; i < rec_seeked.Length; i++)
            {
                rec_seeked[i] = mem_receive.GetBuffer()[i];
            }
            return rec_seeked;
        }

        private void AnswerClient(Socket client, Trace trace)
        {
            MemoryStream mem_answer = new MemoryStream(BUFFER_SIZE);
            // Мой способ - m_serializer.Serialize(mem_answer, trace);
            // Для Влада
            string answer = string.Format("{0},{1},{2}", trace.ID, trace.Method, trace.Properties);
            byte[] b_answer = Encoding.UTF8.GetBytes(answer);
            client.Send(b_answer);
        }

        private void SendTraceToClient(Trace trace, Socket client)
        {
            MemoryStream mem_answer = new MemoryStream(BUFFER_SIZE);
            m_serializer.Serialize(mem_answer, trace);
            client.Send(mem_answer.GetBuffer());
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

        private void AnswerError(Socket client)
        {
            string er = "Error";
            client.Send(Encoding.UTF8.GetBytes(er));
        }
    }
}
