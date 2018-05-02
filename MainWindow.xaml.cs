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
using System.ComponentModel;

namespace BS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> userlist = new List<User>();
        string localName = null;//本机的主机名
        IPAddress[] ipv4 = new IPAddress[6];//本机所有ipv4地址
        User userNow = null;//正在通信的客户端的ip地址及主机名
        User editNick = null;//准备修改昵称的用户
        DetailDAL IntoDB = new DetailDAL();//实例化对DB中消息表的操作类DetailDAL
        NickDAL test = new NickDAL();//实例化对DB中昵称表的操作类DetailDAL
        Detail s = null;//一条需要插入数据库的信息

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

            //ThreadPool.QueueUserWorkItem(CheckUnread);
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
                
                if (cmd == "JOIN")
                {
                    //假设mac地址放在前面
                    string macaddress = message.Substring(4, 17);
                    name = message.Substring(21);
                    try
                    {
                        User remote = new User(ep.Address, name,macaddress);//设置nickname默认为主机名，即u_name
                        remote.MacAdd = macaddress;
                        User tmp = userlist.Find(i =>
                        {
                            if (i.u_ip.Equals(remote.u_ip) && i.u_name.Equals(name))
                                return true;
                            return false;
                        });

                        if (tmp == null)
                        {
                            string Exit = test.Check(remote);
                            if (Exit == null)
                                test.AddNick(remote);
                            else remote.nickname = Exit;
                            //1、查询在不在数据库的表里面
                            //2、在就返回对应的nickname，不在则插入表中
                            //3、设置remote对象的nickname
                            //4、修改xaml文件中的列表绑定
                            userlist.Add(remote);
                            this.Dispatcher.Invoke(new Action(delegate
                            {
                                ListUser.Items.Add(remote);
                            }));  
                        }
                    }
                    catch 
                    {
                        MessageBox.Show("有位朋友下线了"); 
                    }
                }
                else if (cmd == "LEAV")
                {
                    name = message.Substring(4);
                    User remote = new User(ep.Address, name);
                    User tmp = userlist.Find(i =>
                    {
                        if (i.u_ip.Equals(remote.u_ip) && i.u_name.Equals(name))
                            return true;
                        return false;
                    });
                    if (tmp != null)
                    {
                        userlist.Remove(tmp);
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            ListUser.Items.Remove(tmp);
                        }));
                    }
                }
            }
        }

        private void ListUser_SelectionChanged(object sender, SelectionChangedEventArgs e)   //点击出现于其他主机的聊天界面
        {
            userNow = ListUser.SelectedItem as User;
            ListHistory.Visibility = Visibility.Visible;
            ToolBar1.Visibility = Visibility.Visible;
            tb_SendMes.Visibility = Visibility.Visible;
            StackPanel1.Visibility = Visibility.Visible;
            Lv_head.Header ="与 " + userNow.u_name + " 聊天中";//考虑要不要改成nickname
            ThreadPool.QueueUserWorkItem(DisplayOneMessage);
        }

        private void DisplayOneMessage(object state)
        {
            bool completed = false;
            List<Detail> MesList = null;
            List<Display> DisList = new List<Display>();
            string word = null;//将要显示在列表中的信息的头部（除了内容以外的部分）
            while (!completed)
            {
                if (userNow != null)
                {
                    MesList = IntoDB.FindOneUnread(userNow);
                    DisList.Clear();
                    try
                    {
                        if (MesList.Count > 0)
                        {
                            IntoDB.UnreadToRead(userNow);
                            foreach (Detail i in MesList)
                            {
                                word = DateTime.Now.ToLongTimeString().ToString() + " " + i.DetSpeak + " ";
                                if (i.DetType == 0)
                                {
                                    DisList.Add(new Display(0, word+i.DetCont, null));
                                }
                                else if (i.DetType == 1)
                                {
                                    string pathname = i.DetCont;
                                    string filename = pathname.Substring(pathname.LastIndexOf("\\") + 1);
                                    DisList.Add(new Display(1,word+"点击查看" + filename, i.DetCont));//显示文件名
                                }
                                else if (i.DetType == 2)
                                {
                                    DisList.Add(new Display(2, word+"点击查看截图", i.DetCont));
                                }
                            }
                        }
                    }
                    catch { }

                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        try
                        {
                            if (DisList.Count > 0)
                            {
                                foreach (Display i in DisList)
                                {
                                    ListHistory.Items.Add(i);
                                }
                            }
                        }
                        catch { }
                    }));
                }
                else return;
            }
        }

        private void btnSendMessage(object sender, RoutedEventArgs e)
        {
            string content = tb_SendMes.Text.ToString();
            string message = "MESS"+content;
            //MessageBox.Show(message);
            tb_SendMes.Text = null;
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            try
            {
                ipSent = new IPEndPoint(userNow.u_ip, 11000);//设置服务器ip地址和端口
                socketSent = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化套接字：寻址方案，以字符流方式和tcp通信

                socketSent.Connect(ipSent);//与服务器进行连接
                socketSent.Send(bytes);
                socketSent.Close();
                s = new Detail
                {
                    DetUser = userNow.u_name,
                    DetSpeak = localName,
                    DetType = 0,
                    DetCont = content,
                    DetRead = 1,
                };
                IntoDB.AddDetail(s);

                //display the message in the listview
                this.Dispatcher.Invoke(new Action(delegate
                {
                    string mess = DateTime.Now.ToLongTimeString().ToString() + " " + localName + ":" + content;
                    Display newone = new Display(0, mess, null);
                    ListHistory.Items.Add(newone);
                }));
            }
            catch
            {
                MessageBox.Show(userNow.u_name+"已下线");
            }
        }

        private void btnCloseMessage(object sender, RoutedEventArgs e)
        {
            userNow = null;
            ListHistory.Visibility = Visibility.Hidden;
            ToolBar1.Visibility = Visibility.Hidden;
            tb_SendMes.Visibility = Visibility.Hidden;
            StackPanel1.Visibility = Visibility.Hidden;
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
                ChatSession takeChat = new ChatSession(chat,userlist);
                Thread newThread = new Thread(new ThreadStart(takeChat.StartChat));
                newThread.SetApartmentState(ApartmentState.STA); 
                newThread.IsBackground = true;
                newThread.Start();
                
            }

        }

        private void btnSendFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fileDlg = new System.Windows.Forms.OpenFileDialog();
            fileDlg.Title = "文件发送";
            fileDlg.Filter = "文件(*.*)|*.*";

            System.Windows.Forms.DialogResult result = fileDlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    ipSent = new IPEndPoint(userNow.u_ip, 11000);//设置服务器ip地址和端口

                    ClassSendFile sendFile = new ClassSendFile(fileDlg,userNow.u_name,localName);
                    ThreadPool.QueueUserWorkItem(sendFile.SendFile, ipSent);
                }
                catch
                {
                    MessageBox.Show(userNow.u_name + "已下线");
                }
            }
        }

        private void CheckUnread(object state)
        {
            List<string> NameUnread = null;

            bool completed = false;
            while (!completed)
            {
                NameUnread = IntoDB.FindUnread();
                if (NameUnread.Count() > 0)
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        User tmp;
                        if (NameUnread.Count() > 0)
                        {
                            foreach (User i in userlist)
                            {
                                if (NameUnread.Contains(i.u_name))
                                {
                                    if (i.IsMess == null)
                                    {
                                        tmp = i;
                                        i.IsMess = "有";
                                        ListUser.Items.Add(i);
                                        ListUser.Items.Remove(tmp);
                                    }
                                }
                                /*else
                                {
                                    if (i.IsMess.Equals("有"))
                                    {
                                        tmp = i;
                                        i.IsMess = null;
                                        ListUser.Items.Add(i);
                                        ListUser.Items.Remove(tmp);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (User i in userlist)
                            {
                                tmp = i;
                                i.IsMess = null;
                                ListUser.Items.Add(i);
                                ListUser.Items.Remove(tmp);*/
                            }
                        }
                    }));
                }
            }
        }

        private void btnHistory(object sender, RoutedEventArgs e)
        {
            MesHistory showHistory = new MesHistory(userNow);
            showHistory.Show();
        }

        private void ScreenCutter(object sender, RoutedEventArgs e)
        {
            ScreenCut cut = new ScreenCut(userNow.u_name, localName);
            ipSent = new IPEndPoint(userNow.u_ip, 11000);//设置服务器ip地址和端口
            cut.ipsent = ipSent;
            cut.Show();
        }

        private void ListHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Display DetMess = ListHistory.SelectedItem as Display;
            if (DetMess.Type == 1)
            {
                string path = DetMess.Path.Substring(0,DetMess.Path.LastIndexOf("\\"));
                System.Diagnostics.Process.Start("explorer.exe", path);//调用explorer资源管理器打开
            }
            else if (DetMess.Type == 2)
            {
                DisplayPic showPicForm = new DisplayPic(DetMess.Path);
                showPicForm.Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ClassBroadCast broadCast = new ClassBroadCast();        //广播本机退出的消息
            broadCast.Leave(localName);
            return;
        }

        private void ChangeNickName(object sender, RoutedEventArgs e)
        {
            editNick = ListUser.SelectedItem as User;
            tb_EditNick.Visibility = Visibility.Visible;
            bt_CancelEdit.Visibility = Visibility.Visible;
            bt_CompelteEdit.Visibility = Visibility.Visible;
            tb_EditNick.Text = editNick.nickname;
        }

        private void CancelEditNick(object sender, RoutedEventArgs e)
        {
            tb_EditNick.Visibility = Visibility.Hidden;
            bt_CancelEdit.Visibility = Visibility.Hidden;
            bt_CompelteEdit.Visibility = Visibility.Hidden;
        }

        private void CompleteEditNick(object sender, RoutedEventArgs e)
        {
            tb_EditNick.Visibility = Visibility.Hidden;
            bt_CancelEdit.Visibility = Visibility.Hidden;
            bt_CompelteEdit.Visibility = Visibility.Hidden;

            string tmp = tb_EditNick.Text.ToString();
            if (tmp != null)
            {
                try
                {
                    userlist.Remove(editNick);
                    ListUser.Items.Remove(editNick);
                    editNick.nickname = tmp;
                    ListUser.Items.Add(editNick);
                    userlist.Add(editNick);
                    test.EditNick(editNick);
                }
                catch { }
            }

        }

        private void ListUser_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    userNow = ListUser.SelectedItem as User;
                    if (userNow != null)
                    {
                        ListHistory.Visibility = Visibility.Visible;
                        ToolBar1.Visibility = Visibility.Visible;
                        tb_SendMes.Visibility = Visibility.Visible;
                        StackPanel1.Visibility = Visibility.Visible;
                        Lv_head.Header = "与 " + userNow.u_name + " 聊天中";//考虑要不要改成nickname
                        ThreadPool.QueueUserWorkItem(DisplayOneMessage);
                    }
                }
                catch { }
            }
        }
    }
}