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
    class ChatServer
    {
        BinaryFormatter m_serializer;
        CancellationToken m_cancel_token; // TODO: Добавить возможность остановки сервера
        // Слушатель сервера
        private Socket m_listener;
        // Максимальный размер входящего пакета данных
        private const int BUFFER_SIZE = 1024 * 16;
        public event EventHandler<AcceptDataEventArgs> AcceptedClient;

        public ChatServer(int port)
        {
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
            finally
            {
                m_listener.Dispose();
            }
        }

        private Task ListenAndProcedureClientsAsync()
        {
            Task listen_task = Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        AcceptAndProcedureAndAnswerClient();
                    };
                }
            );
            return listen_task;
        }

        // Ожидает клиента, затем считывает Trace-пакет данных, обрабатывает и возвращает ответ.
        private void AcceptAndProcedureAndAnswerClient()
        {
            Socket client = m_listener.Accept();

            MemoryStream mem_receive = new MemoryStream();
            client.Receive(mem_receive.GetBuffer());

            object receive_obj = m_serializer.Deserialize(mem_receive);
            Trace answer = ProcedureReceive(receive_obj);
            AnswerClient(client, answer);
            OnAcceptClient(client, answer);
        }

        private void AnswerClient(Socket client, Trace trace)
        {
            MemoryStream mem_answer = new MemoryStream();
            m_serializer.Serialize(mem_answer, trace);
            client.Send(mem_answer.GetBuffer());
        }

        private void SendTraceToClient(Trace trace, Socket client)
        {
            MemoryStream mem_answer = new MemoryStream();
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
            return new Trace() { Method = "Answer" }; // TODO: Заглушка обработки пакета с запросом.
        }
    }
}
