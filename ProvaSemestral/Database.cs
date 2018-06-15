using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ProvaSemestral
{
    public class Database
    {
        private static string connectionString = "Server=tcp:projetoagenda.database.windows.net,1433;Initial Catalog=agenda;Persist Security Info=False;User ID=Anonimy;Password=Mateuss14la;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public static SqlConnection GetConn()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
