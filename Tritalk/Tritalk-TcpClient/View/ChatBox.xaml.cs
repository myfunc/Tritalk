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

namespace Tritalk.View
{
    /// <summary>
    /// Interaction logic for ChatBox.xaml
    /// </summary>
    public partial class ChatBox : Window
    {
        private UserControl chatbox;

        public ChatBox(UserControl box)
        {
            chatbox = box;
            InitializeComponent();
            AddControl();
        }

        private void AddControl()
        {
            MainView.Children.Add(chatbox);
        }

    }
}
