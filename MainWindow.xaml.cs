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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string localName = null;//本机的主机名
        IPAddress[] ipv4 = new IPAddress[6];//本机所有ipv4地址
        User userNow = null;//正在通信的客户端的ip地址及主机名

        //定义作为服务器端接受信息套接字
        public Socket socketReceive = null;

        //定义作为客户端发送信息套接字
        public Socket socketSent = null;

        //定义接受信息的IP地址和端口号
        public IPEndPoint ipReceive = null;

        //定义发送信息的IP地址和端口号
        public IPEndPoint ipSent = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ConnectionDB.CreateTables();

            IPAddress[] ips = null;
            ComName.Text = Dns.GetHostName();
            localName = Dns.GetHostName();
            ips = Dns.GetHostAddresses(ComName.Text);

            int i = 0;
            foreach (IPAddress one in ips)//筛选，只显示本机IPv4地址
            {
                if (one.AddressFamily == AddressFamily.InterNetwork)
                {
                    ComIP.Items.Add(one);
                    ipv4[i++] = one;
                }
            }
            //新开3个线程，1、监听其它主机的广播消息 2、广播 3、监听其它主机非广播的信息

            ThreadPool.QueueUserWorkItem(StartListenUdp);    //监听其它主机发的广播包

            ClassBroadCast broadCast = new ClassBroadCast();        //广播本机信息
            ThreadPool.QueueUserWorkItem(broadCast.BroadCast,localName);

            ThreadPool.QueueUserWorkItem(ReceiveNews);//监听其它主机非广播的信息
        }

        public void StartListenUdp(object state)                   //监听其它主机发的广播包
        {
            string message = null;
            string cmd = null;
            string name = null;
            UdpClient server = new UdpClient(8001);//在本地8001端口监听
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);//监听任意ip地址的udp报文

            bool completed = false;
            while (!completed)
            {
                byte[] buff = server.Receive(ref ep);
                message = Encoding.Default.GetString(buff);
                cmd = message.Substring(0,4);
                name = message.Substring(4);
                if (cmd == "JOIN")
                {
                    try
                    {
                        User remote = new User(ep.Address, name);
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            bool flag = true;
                            foreach (User i in ListUser.Items)
                            {
                                if (remote.u_name == i.u_name)
                                    flag = false;
                            }
                            if (flag)
                            {
                                ListUser.Items.Add(remote);

                            }
                        }));  
                    }
                    catch 
                    {
                        MessageBox.Show("有位朋友下线了"); 
                    }
                }
                //设置下线通知
            }
        }


        private void ListUser_SelectionChanged(object sender, SelectionChangedEventArgs e)   //点击出现于其他主机的聊天界面
        {
            userNow = ListUser.SelectedItem as User;
            tb_name.Visibility = Visibility.Visible;
            tb_history.Visibility = Visibility.Visible;
            ToolBar1.Visibility = Visibility.Visible;
            tb_SendMes.Visibility = Visibility.Visible;
            StackPanel1.Visibility = Visibility.Visible;
            tb_name.Text = userNow.u_name;

        }

        private void btnSendMessage(object sender, RoutedEventArgs e)
        {
            string message = "MESS"+tb_SendMes.Text.ToString();
            MessageBox.Show(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            try
            {
                ipSent = new IPEndPoint(userNow.u_ip, 11000);//设置服务器ip地址和端口
                socketSent = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化套接字：寻址方案，以字符流方式和tcp通信

                socketSent.Connect(ipSent);//与服务器进行连接
                socketSent.Send(bytes);
                socketSent.Close();
                //MessageBox.Show(userNow.u_name + "发送成功");
            }
            catch
            {
                MessageBox.Show(userNow.u_name+"已下线");
            }
        }

        private void ReceiveNews(object state)
        {
            try
            {
                //初始化接受套接字：寻址方案，以字符流方式和Tcp通信
                socketReceive = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //设置本机IP地址和接收信息的端口，IP地址设为本机所有IP地址
                ipReceive = new IPEndPoint(IPAddress.Any, 11000);
                //将本机IP地址和接受端口绑定到接受套接字
                socketReceive.Bind(ipReceive);
                //监听端口，并设置监听最大连接个数
                socketReceive.Listen(100);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            while (true)
            {
                Socket chat = socketReceive.Accept();//定义接受信息的套接字
                ChatSession takeChat = new ChatSession(chat);
                Thread newThread = new Thread(new ThreadStart(takeChat.StartChat));
                newThread.SetApartmentState(ApartmentState.STA); 
                newThread.IsBackground = true;
                newThread.Start();
                
            }

        }

        private void ThreadStart(Action<object> action)
        {
            throw new NotImplementedException();
        }

        private void btnSendFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDlg = new System.Windows.Forms.OpenFileDialog();
            fileDlg.Title = "文件发送";
            fileDlg.Filter = "文件(*.*)|*.*";

            System.Windows.Forms.DialogResult result = fileDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //MessageBox.Show(fileDlg.FileName);
                try
                {
                    ipSent = new IPEndPoint(userNow.u_ip, 11000);//设置服务器ip地址和端口

                    ClassSendFile sendFile = new ClassSendFile(fileDlg);
                    ThreadPool.QueueUserWorkItem(sendFile.SendFile, ipSent);
                }
                catch
                {
                    MessageBox.Show(userNow.u_name + "已下线");
                }
            }
        }
    }
}