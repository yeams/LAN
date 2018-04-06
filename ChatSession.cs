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
        public ChatSession(Socket chat)
        {
            this.chat = chat;
        }
        public ChatSession()
        {
        }
        public void StartChat()
        {
            //获取远程主机的IP地址和端口号
            IPEndPoint ep = (IPEndPoint)chat.RemoteEndPoint;
            //设置缓冲区
            byte[] buff = new byte[1024];
            int len;
            string fileName = null;//保存文件的文件名

            while ((len = chat.Receive(buff)) != 0)
            {
                string msg = Encoding.Unicode.GetString(buff,0,len);
                string cmd = msg.Substring(0,4);
                System.Windows.Forms.MessageBox.Show(cmd);
                if (cmd == "FILE")
                {
                    DialogResult res = System.Windows.Forms.MessageBox.Show(ep.Address.ToString()+"向你发送文件？","发送文件",MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == res)
                    {
                        System.Windows.Forms.MessageBox.Show("有没有文件呀");
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Title = "文件保存在";
                        saveFileDialog.Filter = "文件(*.*)|*.*";
                        //saveFileDialog.InitialDirectory = @"D:\";//设置保存控件打开后，默认目录
                        int FileNameByteLength = buff[9] * 1024 + buff[8];
                        long FileLength = BytesToInt.byteToLong(buff);

                        saveFileDialog.FileName = msg.Substring(9, FileNameByteLength/2);
                        
                        if(saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (saveFileDialog.FileName != "")
                            {
                                fileName = saveFileDialog.FileName;
                                System.Windows.MessageBox.Show(fileName);
                                //SaveFile(fileName,buff,ref len,ref msg);
                            }
                        }
                    }
                    else System.Windows.Forms.MessageBox.Show("已取消文件");
                }
                else if (cmd == "MESS")
                {
                    string content = msg.Replace("MESS", "");
                    System.Windows.MessageBox.Show(content);
                }
            }
        }
        private void SaveFile(string fileName,byte[] buff,ref int len,ref string msg)
        {
            string tmp = msg.Substring(0,msg.IndexOf(':'));
            fileName += tmp.Substring(tmp.IndexOf('.'), tmp.Length - tmp.IndexOf('.'));
            System.Windows.Forms.MessageBox.Show(fileName);
            FileStream FileWirter = new FileStream(fileName,FileMode.OpenOrCreate,FileAccess.Write);
            FileWirter.Write(buff, 0, len);
            while((len = chat.Receive(buff)) != 0)
            {
                msg = Encoding.Default.GetString(buff, 0, len);
                if (msg == "END") 
                {
                    //System.Windows.Forms.MessageBox.Show(msg);
                    break;
                }
                FileWirter.Write(buff,0,len);
            }
            //FileWirter.Write(buff, 0, len-6);
            FileWirter.Close();
        }

    }
}
