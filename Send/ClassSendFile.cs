using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BS
{
    class ClassSendFile
    {
        private OpenFileDialog fileDlg;
        DetailDAL IntoDB = new DetailDAL();//实例化对数据库的操作类DetailDAL
        Detail s = null;//一条需要插入数据库的信息
        string DBMac = null;
        string DBSpeakName = null;
        public ClassSendFile(OpenFileDialog fileDlg,string name1,string name2)
        {
            this.fileDlg = fileDlg;
            this.DBMac = name1;
            this.DBSpeakName = name2;
        }
        public void SendFile(object state)
        {
            IPEndPoint ipSend = (IPEndPoint)state;
            Socket SendFile = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化套接字：寻址方案，以字符流方式和tcp通信
            SendFile.Connect(ipSend);

            string filePathName = fileDlg.FileName;
            string msg = "FILE";
            SendFile.Send(Encoding.Unicode.GetBytes(msg));

            string fileNameWithExtension = filePathName.Substring(filePathName.LastIndexOf('\\') + 1);//filePathName 为不带路径的，eg. D://hello//hub.txt||fileNameWithExtension 为不带路径的，eg. hub.txt
            byte[] filenamehead = FileNameHead.GetBytes(fileNameWithExtension);
            SendFile.Send(filenamehead);

            FileStream read = new FileStream(fileDlg.FileName, FileMode.Open, FileAccess.Read);

            byte[] buff = new byte[1024];
            int len = 0;
            while ((len = read.Read(buff, 0, 1024)) != 0)
            {
                if(SendFile.Connected)
                    SendFile.Send(buff,0,len,SocketFlags.None);
            }
            SendFile.Close();
            read.Close();

            s = new Detail
            {
                DetMac = DBMac,
                DetSpeak = DBSpeakName,
                DetType = 1,
                DetCont = filePathName,
                DetRead = 0,
            };
            IntoDB.AddDetail(s);
        }
    }
}
