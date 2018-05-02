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
        public string IsMess { get; set; }
        public string MacAdd { get; set; }
        public string nickname { get; set; }
        public User(IPAddress ip, string name,string macadd)
        {
            this.u_ip = ip;
            this.u_name = name;
            this.IsMess = string.Empty;
            this.MacAdd = macadd;
            this.nickname = name;
        }
        public User(IPAddress ip, string name)
        {
            this.u_ip = ip;
            this.u_name = name;
            this.IsMess = string.Empty;
            this.MacAdd = string.Empty;
            this.nickname = name;
        }
    }
}
