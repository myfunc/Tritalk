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
using Tritalk.Core;

namespace Tritalk.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TraceServer server;
        private ChatCore chat;
        private ChatTraceHandler traceHandler;

        public MainWindow()
        {
            InitializeComponent();
            chat = new ChatCore();
            traceHandler = new ChatTraceHandler(chat);
            server = new TraceServer(7770, traceHandler);
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            server.StartListener();
        }

        public void AcceptClientHandler(object sender, AcceptDataEventArgs args)
        {
            txtLog.Text += Environment.NewLine;
            txtLog.Text += string.Format("Trace:\nID: {0}\nMethod: {1}\n Properties: {2}\n FromIP: {3}",
                args.Trace.ID, args.Trace.Method, args.Trace.Properties, args.Client.RemoteEndPoint.ToString());
        }
    }
}
