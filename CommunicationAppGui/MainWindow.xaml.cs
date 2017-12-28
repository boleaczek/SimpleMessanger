using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using CommunicationLib;

namespace CommunicationAppGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Messanger messanger;
        enum MessangerMode
        {
            Server,
            Client
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IpInput.Text.Trim()), int.Parse(PortInput.Text.Trim()));
            CommunicationStartSetButtons(MessangerMode.Client);
            messanger = new Client(endPoint);
            messanger.OnMesageRecieved += DisplayMessage;
            messanger.Start();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            messanger.Stop();
            CommunicationEndSetButtons();
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && MessageInput.Text.Trim() != "")
            {
                MessageOutput.Text += MessageInput.Text +"\n";
                this.Dispatcher.Invoke(() => messanger.Send(MessageInput.Text.Trim()));
                MessageInput.Clear();
            }
        }

        private void CreateServerButton_Click(object sender, RoutedEventArgs e)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IpInput.Text.Trim()), int.Parse(PortInput.Text.Trim()));
            CommunicationStartSetButtons(MessangerMode.Server);
            messanger = new Server(endPoint);
            messanger.OnMesageRecieved += DisplayMessage;
            messanger.Start();
        }

        void DisplayMessage(object sender, Messanger.MessageRecievedEventArgs e)
        {
            this.Dispatcher.Invoke(() => 
            MessageOutput.Text += e.Text.Trim('\0'));
        }

        void CommunicationStartSetButtons(MessangerMode mode)
        {
            ConnectButton.IsEnabled = false;
            CreateServerButton.IsEnabled = false;
            IpInput.IsEnabled = false;
            MessageInput.IsEnabled = true;
        }
        
        void CommunicationEndSetButtons()
        {
            ConnectButton.IsEnabled = true;
            CreateServerButton.IsEnabled = true;
            IpInput.IsEnabled = true;
            MessageInput.IsEnabled = false;
        }
    }
}
