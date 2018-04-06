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
        public ClassSendFile(OpenFileDialog fileDlg)
        {
            this.fileDlg = fileDlg;
        }
        public void SendFile(object state)
        {
            IPEndPoint ipSend = (IPEndPoint)state;
            Socket SendFile = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//初始化套接字：寻址方案，以字符流方式和tcp通信
            SendFile.Connect(ipSend);

            string filePathName = fileDlg.FileName;
            string msg = "FILE";
            string fileNameWithExtension = filePathName .Substring(filePathName.LastIndexOf('\\')+1);
            byte[] fileNameByte = Encoding.Unicode.GetBytes(fileNameWithExtension);

            FileStream read = new FileStream(fileDlg.FileName, FileMode.Open, FileAccess.Read);
            long fileLength = read.Length;

            byte[] fileLengthByte = BitConverter.GetBytes(fileLength);

            ushort fileNameByteLength = (ushort)fileNameByte.Length;
            byte[] fileNameByteLengthByte = BitConverter.GetBytes(fileNameByteLength);
            byte[] head = new byte[18 + fileNameByteLength];


            Array.Copy(Encoding.Unicode.GetBytes(msg),0,head,0,8);
            Array.Copy(fileNameByteLengthByte,0, head,8,2);
            Array.Copy(fileLengthByte,0, head, 10,8);//a.CopyTo(b, 10);//把a的index 10以后的元素复制给b

            SendFile.Send(head);
            byte[] buff = new byte[1024];
            int len = 0;
            while ((len = read.Read(buff, 0, 1024)) != 0)
            {
                SendFile.Send(buff,0,len,SocketFlags.None);
            }
            msg = "END";
            byte[] www = Encoding.Unicode.GetBytes(msg);
            SendFile.Send(Encoding.Unicode.GetBytes(msg));
            SendFile.Close();
            read.Close();


        }
    }
}
