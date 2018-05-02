using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    interface INickDAL
    {
        int AddNick(User obj);//增加
        string Check(User obj);//检查对应mac地址的用户是否存在，存在返回nick，不存在返回null
        int EditNick(User obj);//修改对应mac地址的nick
    }
}
