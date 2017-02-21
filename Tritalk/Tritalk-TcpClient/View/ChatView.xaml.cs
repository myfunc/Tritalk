using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Tritalk.View;

namespace Tritalk.Client
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        private ChatClient chat_client = null;

        public ChatView(ChatClient chatClient)
        {
            InitializeComponent();
            chat_client = chatClient;
        }

        public void BeginChatControl()
        {
            Update();
        }

        private async void Update()
        {
            string[] users = (await chat_client.GetUsers()).users;
            lbxUsers.ItemsSource = users;
        }

        private void lbxUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbxUsers.SelectedItem == null) return;
            string selected_user = lbxUsers.SelectedItem as string;
            OpenChat(selected_user);
        }

        private void OpenChat(string user)
        {
            ChatBoxControl cbc = new ChatBoxControl(chat_client, user);
            ChatBox cbox = new ChatBox(cbc);
            cbox.Show();
        }
    }
}
