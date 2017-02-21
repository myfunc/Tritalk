using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tritalk.Client;
using Tritalk.Core.ChatProtocolObjects;

namespace Tritalk.View
{
    /// <summary>
    /// Interaction logic for ChatBoxControl.xaml
    /// </summary>
    public partial class ChatBoxControl : UserControl, IDisposable
    {
        private ChatClient chat_client = null;
        private string dialog_user;

        private readonly string status_template = "Chat: {0} - {1}";
        private readonly string message_template = "{0} {1}:{2}\n";

        Task auto_update_messages = null;

        public ChatBoxControl(ChatClient chat, string dialogPerson)
        {
            chat_client = chat;
            dialog_user = dialogPerson;
            InitializeComponent();
            Init();
            Update();
        }

        public void Init()
        {
            chat_client.ReceivedData += NewMessageHandler;
            auto_update_messages = CheckNewMessageAsync();
        }

        private void Update()
        {
            UpdateStatus();
            chat_client.GetNewMessages();
        }

        private void AddMessage(ProtoMessage message)
        {
            txtChatlog.Text += string.Format(message_template, message.time, message.from, message.text);
        }

        private void UpdateNewMessages(params ProtoMessage[] list)
        {
            foreach (var i in list)
            {
                Dispatcher.Invoke(() => AddMessage(i));
                chat_client.IReadMessage(i.message_id);
            }
        }

        async Task CheckNewMessageAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    chat_client.GetNewMessages();
                }
            });
        }

        private void NewMessageHandler(object sender, ReceivedDataEventArgs args)
        {
            if (args.Trace.Method == "GetNewMessages")
            {
                ProtoMessageList list = ProtocolObject.FromJSON<ProtoMessageList>(args.Trace.Parameters);
                List<ProtoMessage> filtered_list = new List<ProtoMessage>();
                foreach (var i in list.messages)
                {
                    if (i.from == dialog_user)
                    {
                        filtered_list.Add(i);
                    }
                }
                UpdateNewMessages(filtered_list.ToArray());
                
            }
        }

        private async void SendMessage()
        {
            if ((await chat_client.SendMessage(dialog_user, txtMessage.Text)).success)
            {
                AddMessage(new ProtoMessage() { time = DateTime.Now.ToString(), from = chat_client.AuthUser.Name, text = txtMessage.Text });
                txtMessage.Clear();
            } else
            {
                MessageBox.Show("Some error");
            }
        }

        private void UpdateStatus()
        {
            txtStatus.Text = string.Format(status_template, chat_client.AuthUser.Name, dialog_user);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        public void Dispose()
        {
            auto_update_messages.Dispose();
        }
    }
}
