using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    public interface IDetailDAL
    {
        int AddDetail(Detail obj);//增加新的信息 insert
        int EditDetail(Detail obj);//更新信息 update
        int DeleteAll();//删除所有的聊天记录
        int DeleteOne(User obj);//删除与某个人的聊天记录
        int UnreadToRead(User obj);//修改对应未读变已读
        List<Detail> FindOne(User obj);//查找与某个人的聊天信息
        List<Detail> FindOneUnread(User obj);//查找与某人聊天的未读信息
        List<string> FindUnread();//查找没有读过的聊天信息

    }
}
