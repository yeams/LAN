using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace BS
{
    public abstract class SqlHelper
    {
        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] cmdParms)
        {
            SQLiteConnection con = ConnectionDB.Connect();
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            foreach (SQLiteParameter parm in cmdParms)
            {
                cmd.Parameters.Add(parm);
            }
            try
            {
                con.Open();
                int num = cmd.ExecuteNonQuery();
                return num;
            }
            catch
            {
                return 0;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }
        public static DataTable ExecuteQuery(string sql, params SQLiteParameter[] cmdParms)
        {
            SQLiteConnection con = ConnectionDB.Connect();
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            foreach (SQLiteParameter parm in cmdParms)
            {
                cmd.Parameters.Add(parm);
            }
            DataTable dt = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
            }
        }
        public static DataTable ExecuteQuery(out int total, string sql, string sortExpression, int startRowIndex, int maximumRows, params SQLiteParameter[] cmdParms)
        {
            string sort = String.IsNullOrEmpty(sortExpression) ? "" : " order by " + sortExpression;
            string sql1 = "select count(*)" + sql.Substring(sql.IndexOf(" from"));
            SQLiteConnection con = ConnectionDB.Connect();
            SQLiteCommand cmd = new SQLiteCommand(sql + sort, con);
            SQLiteCommand cmd1 = new SQLiteCommand(sql1, con);
            con.Open();
            foreach (SQLiteParameter parm in cmdParms)
            {
                cmd.Parameters.Add(parm);
                cmd1.Parameters.Add(parm);
            }
            total = Int32.Parse(cmd.ExecuteScalar().ToString());
            DataTable dt = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            try
            {
                da.Fill(startRowIndex, maximumRows, dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
                cmd1.Dispose();
                con.Close();
            }
        }
        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] cmdParms)
        {
            SQLiteConnection con = ConnectionDB.Connect();
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            foreach (SQLiteParameter parm in cmdParms)
            {
                cmd.Parameters.Add(parm);
            }
            try
            {
                con.Open();
                SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch
            {
                return null;
            }
        }
        public static object ExecuteScalar(string sql, params SQLiteParameter[] cmdParms)
        {
            SQLiteConnection con = ConnectionDB.Connect();
            SQLiteCommand cmd = new SQLiteCommand(sql, con);
            try
            {
                con.Open();
                object val = cmd.ExecuteScalar();
                return val;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
                con.Dispose();
            }
        }
    }
}
