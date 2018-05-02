using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Management;

namespace BS
{
    class ClassBroadCast
    {
        public bool completed = false;
        public void BroadCast(object state)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 8001);

            //GetMacAddress(); 加到数据包里面
            string message = "JOIN" + GetMacAddress() + (string)state;
            byte[] buff = Encoding.Default.GetBytes(message);
            while (!completed)
            {
                udpClient.Send(buff, buff.Length, ep);
                Thread.Sleep(2000);
            }
            udpClient.Close();
            return;
        }
        public void Leave(string state)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 8001);
            string message = "LEAV" + state;
            byte[] buff = Encoding.Default.GetBytes(message);
            udpClient.Send(buff,buff.Length,ep);
            udpClient.Close();
            return;
        }
        private static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                        Console.WriteLine(strMac);
                        break;
                    }
                }
                moc.Dispose();
                mc.Dispose();
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
