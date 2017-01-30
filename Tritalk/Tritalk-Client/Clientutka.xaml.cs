using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using Tritalk.Libs;

namespace Tritalk_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Trace User;
        string MyName;
        string MyId;

        const int port = 7770;
        const string iphost = "127.0.0.1";



        public MainWindow()
        {
            InitializeComponent();
            User = new Trace();
        }

        private void MakeAutorisationAlone(object sender, RoutedEventArgs e)
        {
            MakeAutAlone();
        }

        private void MakeAutAlone()
        {
            lbFriends.Visibility = Visibility.Hidden;
            lbFaqFriends.Visibility = Visibility.Hidden;
            tBlockMessages.Text = "";
            lbName.Visibility = Visibility.Hidden;
            lbFaq.Visibility = Visibility.Hidden;
            tBlockMessages.Visibility = Visibility.Hidden;
            tbCreaterMessages.Visibility = Visibility.Hidden;
            btnSendMessage.Visibility = Visibility.Hidden;
        }

        private void MaskerovkaOff()
        {
            lbFaqFriends.Visibility = Visibility.Visible;
            lbFriends.Visibility = Visibility.Visible;
            lbName.Visibility = Visibility.Visible;
            lbFaq.Visibility = Visibility.Visible;
            tBlockMessages.Visibility = Visibility.Visible;
            tbCreaterMessages.Visibility = Visibility.Visible;
            btnSendMessage.Visibility = Visibility.Visible;


            tbLogin.Visibility = Visibility.Hidden;
            pbPassword.Visibility = Visibility.Hidden;
            lbAutorization.Visibility = Visibility.Hidden;
            lbLogin.Visibility = Visibility.Hidden;
            lbPassword.Visibility = Visibility.Hidden;
            btnIn.Visibility = Visibility.Hidden;

        }


        string GetPropFromloginAndPassword()
        {
            string[] msg = null;
            msg = new string[2];
            msg[0] = tbLogin.Text;
            msg[1] = pbPassword.Password;
            return String.Join("**", msg);

        }
        private void GetAutorization(object sender, RoutedEventArgs e)
        {
            try
            {
                User.ID = "User";
                User.Method = "Authorization";
                User.Parameters = GetPropFromloginAndPassword();
                SendMessageFromSocket(port, User);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Сервак не работает");
            }
        }
        private void SendMessage(object sender, RoutedEventArgs e)
        {
            try
            {
                User.ID = MyId;
                User.Method = "SendMessage";
                User.Parameters = tbCreaterMessages.Text;
                tBlockMessages.Text += "\n" + MyName + ": " + tbCreaterMessages.Text;
                SendMessageFromSocket(port, User);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Сервак не работает");
            }
        }
        void SendMessageFromSocket(int port, Trace trace)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[1024];

            // Соединяемся с удаленным устройством

            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry(iphost);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);

            //отправляем сообщение
            string message;
            byte[] msg;
            int bytesSent;
            //Че мы вааще хотим
            switch (trace.Method)
            {
                case "Authorization"://AutorizationProverka


                    message = MakeLineTrace(trace);
                    msg = Encoding.UTF8.GetBytes(message);
                    bytesSent = sender.Send(msg);


                    int bytesRep1 = sender.Receive(bytes);
                    //предполагается что ты мне вернешь
                    //1 -ID - мой (Юзера)
                    //2 = metodname- NameUsera
                    //3 -Prop =  true or false 

                    string mess = Encoding.UTF8.GetString(bytes, 0, bytesRep1);
                    SplitReplyAutorization(mess);
                    break;
                case "SendMessage"://Сейчас Бахаю
                    message = MakeLineTrace(trace);
                    msg = Encoding.UTF8.GetBytes(message);
                    bytesSent = sender.Send(msg);
                    //предполагается что ты мне вернешь
                    //1 - не важно
                    //2 = MethodName Тоже всеравно
                    //3 = Prop стринга Что сервер получил сообщение
                    int bytesRec = sender.Receive(bytes);
                    string rep = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    tBlockMessages.Text += "\n Server: " + rep;
                    break;
                default:
                    MessageBox.Show("Чет пошло не так , Возможно ты натупил с сикретом");
                    break;
            }
            // Освобождаем сокет
            sender.Close();
        }
        private string MakeLineTrace(Trace trace)
        {
            string[] msg = null;
            msg = new string[3];
            msg[0] = trace.ID;
            msg[1] = trace.Method;
            msg[2] = trace.Parameters;
            return String.Join(",", msg);
        }

        Trace GetTrace(Trace Rep)
        {
            User.ID = Rep.ID;
            User.Method = Rep.Method;
            User.Parameters = User.Parameters;
            return User;
        }
        void SplitReplyAutorization(string rep)
        {
            string[] result;
            string[] stringSeparators = new string[] { "**" };
            result = rep.Split(stringSeparators, StringSplitOptions.None);

            User.ID = result[0];
            User.Method = result[1];
            User.Parameters = result[2];
            if (User.Parameters == true.ToString())
            {
                //Делаем приличный вид чатику
                MaskerovkaOff();

                MyId = User.ID;
                MyName = User.Method;
                lbName.Content = MyName;
            }
            if (User.Parameters == false.ToString())
            {
                MessageBox.Show("Логин или пароль неверен", "Проверка На вшивость",
                           MessageBoxButton.OK,
                             MessageBoxImage.Error);
                pbPassword.Password = "";
            }

        }




    }
}
