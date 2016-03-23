using System;
using System.Windows;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Org.Mentalis.Network.ProxySocket;

namespace proxyClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            replyText.Text = "";
            try
            {
                var proxy = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                proxy.ProxyEndPoint = new IPEndPoint(IPAddress.Parse(proxyAddress.Text), int.Parse(proxyPort.Text));
                proxy.ProxyUser = proxyUser.Text;
                proxy.ProxyPass = proxyPass.Text;

                proxy.ProxyType = ProxyTypes.Socks5;
                //var proxy = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                proxy.Connect(hostAddress.Text, int.Parse(hostPort.Text));
                var text = Encoding.ASCII.GetBytes(postText.Text);
                proxy.Send(text);

                int recv = 0;
                byte[] buffer = new byte[1024];
                recv = proxy.Receive(buffer);
                while (recv > 0)
                {
                    replyText.Text += Encoding.ASCII.GetString(buffer, 0, recv);
                    recv = proxy.Receive(buffer);
                }
            }
            catch (SocketException ex)
            {
                replyText.Text = ex.ToString();
                return;
            }
            if (replyText.Text.Length == 0)
            {
                replyText.Text = "No reply";
            }
        }
    }
}
