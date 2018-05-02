using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    class ClassSendPic
    {
        private string filePathName;
        DetailDAL IntoDB = new DetailDAL();//实例化对数据库的操作类DetailDAL
        Detail s = null;//一条需要插入数据库的信息
        string DBUserName = null;
        string DBSpeakName = null;

        public ClassSendPic(string _filename, string name1, string name2)
        {
            this.filePathName = _filename;
            this.DBUserName = name1;
            this.DBSpeakName = name2;
        }
        public void SendFile(object state)
        {
            IPEndPoint ipSend = (IPEndPoint)state;
            Socket SendFile = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化套接字：寻址方案，以字符流方式和tcp通信
            SendFile.Connect(ipSend);

            string msg = "PICT";
            SendFile.Send(Encoding.Unicode.GetBytes(msg));

            FileStream read = new FileStream(filePathName, FileMode.Open, FileAccess.Read);

            byte[] buff = new byte[1024];
            int len = 0;
            while ((len = read.Read(buff, 0, 1024)) != 0)
            {
                if (SendFile.Connected)
                    SendFile.Send(buff, 0, len, SocketFlags.None);
            }
            SendFile.Close();
            read.Close();

            s = new Detail
            {
                DetUser = DBUserName,
                DetSpeak = DBSpeakName,
                DetType = 2,
                DetCont = filePathName,
                DetRead = 0,
            };
            IntoDB.AddDetail(s);
        }
    }
}
