using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    class BytesToInt
    {
        public static long byteToLong(byte[] bytes)
        {
            int i = 0;
            long ret = 0;
            for (i = 0; i < 8; i++)
            {
                ret += bytes[i+10] * (1 << (i * 8));
            }
            return ret;
        }
    }
}
