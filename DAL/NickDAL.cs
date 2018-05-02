using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    class NickDAL:INickDAL
    {
        public int AddNick(User obj)
        {
            string add = string.Format("insert into Nick(MacAdd, Nickname) values('{0}','{1}')", obj.MacAdd, obj.u_name);
            return SqlHelper.ExecuteNonQuery(add);
        }

        public string Check(User obj)
        {
            string ans = null;
            string sql = String.Format("select Nickname from Nick where MacAdd='{0}'", obj.MacAdd);
            DataTable dt = SqlHelper.ExecuteQuery(sql);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ans = row["Nickname"].ToString();
                    break;
                }
            }
            dt.Dispose();
            return ans;
        }

        public int EditNick(User obj)
        {
            string add = string.Format("UPDATE Nick SET Nickname='{0}' WHERE MacAdd='{1}'", obj.nickname,obj.MacAdd);
            return SqlHelper.ExecuteNonQuery(add);
        }
    }
}
