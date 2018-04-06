using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace BS
{
    class ClassBroadCast
    {
        public bool completed = false;
        public void BroadCast(object state)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 8001);

            string message = "JOIN"+(string)state;
            byte[] buff = Encoding.Default.GetBytes(message);
            while (!completed)
            {
                udpClient.Send(buff, buff.Length, ep);
                Thread.Sleep(2000);
            }
        }
    }
}
