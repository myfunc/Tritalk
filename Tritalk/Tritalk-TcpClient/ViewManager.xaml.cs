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
using System.Windows.Shapes;

namespace Tritalk.Client
{
    /// <summary>
    /// Interaction logic for ViewManager.xaml
    /// </summary>
    public partial class ViewManager : Window
    {
        readonly string connection_uri = "localhost";
        readonly int connection_port = 7770;

        private ChatClient chat_client = null;
        private TraceClient trace_client = null;

        private Auth uc_auth;
        private ChatView uc_chatview;

        public ViewManager()
        {
            InitializeComponent();
            Init();
            BeginAuthorization();
        }

        private void Init()
        {
            InitClients(); // 1st
            InitControls(); // 2nd (require init'ed clients)
        }

        private void InitControls()
        {
            uc_auth = new Auth();
            uc_auth.btnLogin.Click += LoginButton;
            uc_auth.btnSignIn.Click += RegistrationButton;
            uc_chatview = new ChatView(chat_client);
            uc_chatview.btnLogout.Click += LogoutButton;

            txtAdress.Text = connection_uri;
            txtPort.Text = connection_port.ToString();
        }

        private void InitClients()
        {
            trace_client = new TraceClient();
            chat_client = new ChatClient(trace_client);
        }

        private void Start()
        {
            trace_client.Connect(txtAdress.Text, Convert.ToInt32(txtPort.Text));
            trace_client.StartAdapter();
        }

        private void Stop()
        {
            trace_client.StopAdapter();
        }

        private void ClearView()
        {
            MainView.Children.Clear();
        }

        private void BeginAuthorization()
        {
            ClearView();
            MainView.Children.Add(uc_auth);
        }

        private void BeginChatClient()
        {
            ClearView();
            MainView.Children.Add(uc_chatview);
            uc_chatview.BeginChatControl();
            uc_chatview.lblUser.Content = "User: " + chat_client.AuthUser.Name;
        }

        private async void LoginButton(object sender, EventArgs args)
        {
            try
            {
                if (!trace_client.Connected) Start();
                if (await chat_client.Authorization(uc_auth.txtLogin.Text, uc_auth.txtPassword.Text))
                {
                    BeginChatClient();
                }
                else
                {
                    MessageBox.Show("Invalid login or password. Please try again", "Invalid login", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } catch
            {
                MessageBox.Show("Invalid server adress", "Invalid adress", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RegistrationButton(object sender, EventArgs args)
        {
            try
            {
                if (!trace_client.Connected) Start();
                if ((await chat_client.Registration(uc_auth.txtLogin.Text, uc_auth.txtLogin.Text, uc_auth.txtPassword.Text)).success)
                {
                    MessageBox.Show("Registration complite. Please, log in system.", "Registration complite", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Invalid login or password. Please try again", "Invalid login", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Invalid server adress", "Invalid adress", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutButton(object sender, EventArgs args)
        {
            chat_client.Logout();
            BeginAuthorization();
        }
    }
}
