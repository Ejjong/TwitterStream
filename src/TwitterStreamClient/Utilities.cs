using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamClient
{
    public class Utilities
    {
        private static readonly ConnectionStringSettings Connection = ConfigurationManager.ConnectionStrings["nancyDb"];
        private static readonly ConnectionStringSettings BackupConnection = ConfigurationManager.ConnectionStrings["apphbDb"];
        private static readonly string ConnectionString = Connection.ConnectionString;
        private static readonly string BackupConnectionString = BackupConnection.ConnectionString;

        public static SqlConnection GetOpenConnection(bool isBackup = false)
        {
            var connection = new SqlConnection((isBackup ? BackupConnectionString : ConnectionString));
            connection.Open();

            return connection;
        }
    }
}
