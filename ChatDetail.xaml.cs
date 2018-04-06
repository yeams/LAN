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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BS
{
    /// <summary>
    /// ChatDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ChatDetail : Window
    {
        //主窗口传递的远方主机的信息
        private User remoteUser;

        //定义作为客户端发送信息的套接字
        public Socket socketSend = null;
        ///定义发送信息的IP地址和端口号
        public IPEndPoint ipSent = null;

        public bool completed = false; 
        public ChatDetail(User remoteUser)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.remoteUser = remoteUser;
            tb_name.Text = remoteUser.u_name;
            tb_IPadd.Text = remoteUser.u_ip.ToString();

            ipSent = new IPEndPoint(remoteUser.u_ip,11000);    //设置接收方的IP地址和端口

        }

        private void btn_SendMessage(object sender, RoutedEventArgs e)
        {
            string message = tb_SendMes.Text.ToString();
            try
            {
                socketSend = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                socketSend.Connect(ipSent);

                pro_Head head = new pro_Head(pro_Head.MS_Type.MT_MESG, message, Dns.GetHostName());
                byte[] buff = Transform.HeadTobyte(head);
                socketSend.Send(buff);

                socketSend.Close();
            }
            catch
            {
                MessageBox.Show("对方已下线");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            completed = true;
        }
    }
}
