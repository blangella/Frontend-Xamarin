using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace StockUp
{
    public class DBConncection
    {
        private static MySqlConnection con;
        private DBConncection(){ }

        //Open a connection
        public static void OpenCon()
        {
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        //Close a Connection
        public static void CloseCon()
        {
            if(con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        public static MySqlConnection GetCon()
        {
            if(con == null)
            {
                con = new MySqlConnection("Server=db431.cjeog1zo7yyf.us-east-2.rds.amazonaws.com;Port=3306;Database=SIS;Uid=master431;Pwd=masterhot1;");
            }
            return con;
        }

        public static string Login(string user, string pass)
        {
            if (con == null) { con = GetCon(); }
            OpenCon();
            MySqlCommand cmd = new MySqlCommand(cmdText: "SELECT IsAdmin FROM user WHERE Emp_id=@username AND Password=@password", connection: con);
            MySqlParameter username = new MySqlParameter(); username.ParameterName = "@username"; username.Value = user; cmd.Parameters.Add(username);
            MySqlParameter password = new MySqlParameter(); password.ParameterName = "@password"; password.Value = pass; cmd.Parameters.Add(password);
            var un = cmd.ExecuteScalar();
            CloseCon();

            if (un != null)
            {
                un = un.ToString();
                return (string)un;
            }

            return null;
        }
    }
}
