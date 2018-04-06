using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace BS
{
    public class User
    {
        public IPAddress u_ip { get; set; }
        public string u_name { get; set; }
        public User(IPAddress ip, string name)
        {
            this.u_ip = ip;
            this.u_name = name;
        }
    }
}
