using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    public class Transform
    {
        public static string byteToHead(byte[] buff)
        {
            string head;
            MemoryStream stream = new MemoryStream();
            stream.Write(buff, 0, buff.Length);
            IFormatter serializer = new BinaryFormatter();
            stream.Position = 0;
            head = serializer.Deserialize(stream) as string;
            stream.Close();
            return head;
        }
        public static byte[] HeadTobyte(string head)
        {
            IFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, head);
            byte[] buff = stream.ToArray();
            stream.Close();

            return buff;
        }
    }
}
