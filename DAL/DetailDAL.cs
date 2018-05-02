﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS
{
    public class DetailDAL :IDetailDAL
    {
        public int AddDetail(Detail obj)
        {
            string add = string.Format("insert into Detail(Duser, Dspeak, Dtype, Dcont, Dread) values('{0}','{1}','{2}','{3}','{4}')", obj.DetUser, obj.DetSpeak, obj.DetType, obj.DetCont,obj.DetRead);
            return SqlHelper.ExecuteNonQuery(add);
        }

        public int EditDetail(Detail obj)
        {
            string edit = String.Format("update Detail set Dread='{0}' where Did='{1}'", obj.DetRead, obj.DetId);
            return SqlHelper.ExecuteNonQuery(edit);
        }

        public int DeleteAll()
        {
            string delete = String.Format("delete * from Detail");
            return SqlHelper.ExecuteNonQuery(delete);
        }

        public int DeleteOne(User obj)
        {
            string delete = String.Format("delete from Detail where Duser='{0}'", obj.u_name);
            return SqlHelper.ExecuteNonQuery(delete);
        }
        public int UnreadToRead(User obj)
        {
            string change = String.Format("update Detail set Dread=1 where Duser='{0}' and Dread=0", obj.u_name);
            return SqlHelper.ExecuteNonQuery(change);
        }
        public List<Detail> FindOne(User obj)
        {
            List<Detail> list = new List<Detail>();
            string sql = String.Format("select * from Detail where Duser='{0}' ORDER BY Ddatetime ASC", obj.u_name);
            DataTable dt = SqlHelper.ExecuteQuery(sql);
            if (dt != null)
            {
                Detail s = null;
                foreach (DataRow row in dt.Rows)
                {
                    s = new Detail
                    {
                        DetId = Convert.ToInt32(row["Did"]),
                        DetUser = row["Duser"].ToString(),
                        DetSpeak = row["Dspeak"].ToString(),
                        DetType = Convert.ToInt32(row["Dtype"]),
                        DetCont = row["Dcont"].ToString(),
                        DetRead = Convert.ToInt32(row["Dread"]),
                        DetDatetim = (DateTime)row["Ddatetime"]
                    };
                    list.Add(s);
                }
            }
            return list;
        }
        public List<Detail> FindOneUnread(User obj)
        {
            List<Detail> list = new List<Detail>();
            string sql = String.Format("select * from Detail where Duser='{0}' and Dread=0 ORDER BY Ddatetime ASC", obj.u_name);
            DataTable dt = SqlHelper.ExecuteQuery(sql);
            if (dt != null)
            {
                Detail s = null;
                foreach (DataRow row in dt.Rows)
                {
                    s = new Detail
                    {
                        DetId = Convert.ToInt32(row["Did"]),
                        DetUser = row["Duser"].ToString(),
                        DetSpeak = row["Dspeak"].ToString(),
                        DetType = Convert.ToInt32(row["Dtype"]),
                        DetCont = row["Dcont"].ToString(),
                        DetRead = Convert.ToInt32(row["Dread"]),
                        DetDatetim = (DateTime)row["Ddatetime"]
                    };
                    list.Add(s);
                }
            }
            return list;
        }
        public List<string> FindUnread()
        {
            List<string> list = new List<string>();
            string sql = String.Format("select DISTINCT Duser from Detail where Dread = 0");
            DataTable dt = SqlHelper.ExecuteQuery(sql);
            if (dt != null)
            {
                string s = null;
                foreach (DataRow row in dt.Rows)
                {
                    s = row["Duser"].ToString();
                    list.Add(s);
                }
            }
            return list;
        }
    }
}
