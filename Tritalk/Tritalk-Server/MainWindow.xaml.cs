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
using Tritalk.Log;

namespace Tritalk.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TraceTcpServer server;
        private ChatCore chat;
        private LogTraceHandler traceHandler;

        public MainWindow()
        {
            InitializeComponent();
            chat = new ChatCore();
            traceHandler = new LogTraceHandler(new ChatTraceHandler(chat));
            traceHandler.Action += TraceHandler_Action;
            server = new TraceTcpServer(Convert.ToInt32(txtPort.Text), traceHandler);
            
        }

        private void TraceHandler_Action(object sender, DataRequestEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                txtLog.Text += e.Data + Environment.NewLine;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            server.StartListener();
        }
    }
}
