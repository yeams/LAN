using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Forms;
using System.IO;

namespace BS
{
    class ChatSession
    {
        private Socket chat;
        List<User> userlist = null;
        IPEndPoint ep = null;//远程主机的IP地址和端口号
        User ghost = null;//远程主机的主机名
        Detail s = null;//一条需要插入数据库的信息
        DetailDAL IntoDB = new DetailDAL();//实例化对数据库的操作类DetailDAL

        public ChatSession(Socket chat, List<User> userlist)
        {
            this.chat = chat;
            this.userlist = userlist;
        }

        public void StartChat()
        {
            //获取远程主机的IP地址和端口号
            ep = (IPEndPoint)chat.RemoteEndPoint;
            ghost = userlist.Find(tmp =>  //tmp是变量，代表的是userlist中元素
            {
                if (tmp.u_ip.Equals(ep.Address))
                    return true;
                return false;
            });
            //设置缓冲区
            byte[] buff = new byte[1024];
            int len;
            string fileName = null;//保存文件的文件名
            string ExtensionOfFile = null;
            int fileNameByteLength = 8;//保存文件的文件名的byte数长

            while ((len = chat.Receive(buff)) != 0)
            {
                string msg = Encoding.Unicode.GetString(buff,0,len);
                string cmd = msg.Substring(0,4);
                //System.Windows.Forms.MessageBox.Show(cmd);
                if (cmd == "FILE")
                {
                    DialogResult res = System.Windows.Forms.MessageBox.Show(ghost.u_name + "向你发送文件？", "发送文件", MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == res)
                    {
                        fileNameByteLength = FileNameHead.GetFileNameByteLength(buff);
                        fileName = FileNameHead.GetFileName(buff, fileNameByteLength);
                        ExtensionOfFile = fileName.Substring(fileName.LastIndexOf('.'));//得到文件名的扩展名

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Title = "文件保存在";
                        saveFileDialog.Filter = "文件(*" + ExtensionOfFile + ")|*" + ExtensionOfFile + "";
                        saveFileDialog.FileName = fileName.Replace(ExtensionOfFile,"");
                        //saveFileDialog.InitialDirectory = @"D:\";//设置保存控件打开后，默认目录

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (saveFileDialog.FileName != "")
                            {
                                fileName = saveFileDialog.FileName;
                                SaveFile(fileName, buff,12+fileNameByteLength, ref len);
                            }
                        }
                        else while ((len = chat.Receive(buff)) != 0) { }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("已取消文件");
                        while ((len = chat.Receive(buff)) != 0) { }
                    }
                    chat.Close();
                    break;
                }
                else if (cmd == "MESS")
                {
                    string content = msg.Replace("MESS", "");
                    //System.Windows.MessageBox.Show(content);
                    s = new Detail
                    {
                        DetMac = ghost.MacAdd,
                        DetSpeak = ghost.u_name,
                        DetType = 0,
                        DetCont = content,
                        DetRead = 0,
                    };
                    IntoDB.AddDetail(s);
                    chat.Close();
                    break;
                }
                else if (cmd == "PICT")
                {
                    //1、确认pic文件夹的存在
                    string path = Environment.CurrentDirectory + "\\pic";
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(path);
                        directoryInfo.Create();
                    }
                    //2、设置文件名
                    fileName = path + "\\" + DateTime.Now.ToFileTime().ToString() + ".png";
                    //3、接收图片
                    FileStream FileWirter = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                    FileWirter.Write(buff, 8, len - 8);//此方法返回时包含指定的字节数组，数组中 offset 和 (offset +count -1) 之间的值被从当前源中读取的字节替换。
                    while (chat.Connected)
                    {
                        len = chat.Receive(buff);
                        if (len != 0)
                            FileWirter.Write(buff, 0, len);
                        else break;
                    }
                    FileWirter.Close();
                    //4、收发图片的信息插入数据库！！！！！！
                    s = new Detail
                    {
                        DetMac = ghost.MacAdd,
                        DetSpeak = ghost.u_name,
                        DetType = 2,
                        DetCont = fileName,
                        DetRead = 0,
                    };
                    IntoDB.AddDetail(s);
                    chat.Close();
                    break;
                }
            }
            
        }
        private void SaveFile(string fileName, byte[] buff, int HeadLength, ref int len)
        {
            FileStream FileWirter = new FileStream(fileName,FileMode.OpenOrCreate,FileAccess.Write);
            FileWirter.Write(buff, HeadLength, len - HeadLength);//此方法返回时包含指定的字节数组，数组中 offset 和 (offset +count -1) 之间的值被从当前源中读取的字节替换。
            while(chat.Connected)
            {
                len = chat.Receive(buff);
                if (len != 0)
                    FileWirter.Write(buff, 0, len);
                else break;
            }
            FileWirter.Close();
            s = new Detail
            {
                DetMac = ghost.MacAdd,
                DetSpeak = ghost.u_name,
                DetType = 1,
                DetCont = fileName,
                DetRead = 0,
            };
            IntoDB.AddDetail(s);
        }

    }
}
