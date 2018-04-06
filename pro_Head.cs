using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace BS
{
    [Serializable]
    public class pro_Head//协议头格式
    {
        public MS_Type m_Type;              //信息类型
        public string FsName { get; set; }  //发送此信息的用户的用户名
        public int nDataLength;             //后面数据的长度

        public enum MS_Type//定义信息类型
        {
            MT_JOIN,
            MT_LEAVE,
            MT_MESG,  //用户发送信息
        }
        public pro_Head(MS_Type type, int nDataLength, string hostName)
        {
            this.m_Type = type;
            this.nDataLength = nDataLength;
            FsName = hostName;
        }
    }
}
