using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BS
{
    public class ConnectionDB
    {
        private static string connectString = "Data Source =" + Environment.CurrentDirectory + "/history.db";
        public static SQLiteConnection Connect()
        {
            SQLiteConnection conn = new SQLiteConnection(connectString);
            return conn;
        }
        public static void CreateTables()
        {
            SQLiteConnection conn = Connect();
            SQLiteCommand cmdCreateTable = null;
            conn.Open();

            string detail = "CREATE TABLE IF NOT EXISTS Detail(Did INTEGER PRIMARY KEY AUTOINCREMENT, Duser varchar(30) NOT NULL, Dspeak varchar(30) NOT NULL, Dtype INTEGER NOT NULL, Dcont varchar(150) NOT NULL, Dread INTEGER NOT NULL, Ddatetime datetime NOT NULL default (datetime('now', 'localtime')));";
            cmdCreateTable = new SQLiteCommand(detail, conn);
            cmdCreateTable.ExecuteNonQuery();

            string nick = "CREATE TABLE IF NOT EXISTS Nick(MacAdd varchar(40) PRIMARY KEY NOT NULL, Nickname varchar(40) NOT NULL);";
            cmdCreateTable = new SQLiteCommand(nick, conn);
            cmdCreateTable.ExecuteNonQuery();

            cmdCreateTable.Dispose();
            conn.Close();
        }
    }
}
