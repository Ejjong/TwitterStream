using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamClient
{
    public static class Utilities
    {
        private static readonly IConfig config = new Config();
        private static readonly string ConnectionString = config.Get("connectionString");

        public static IDbConnection GetOpenConnection()
        {
            string connString = string.Empty;
            IOrmLiteDialectProvider provider = null;
            if (ConnectionString.StartsWith("postgres://"))
            {
                connString = GenerateConnectionString(connString);
                provider = PostgreSqlDialect.Provider;
            }
            else
            {
                connString = ConnectionString;
                var providerString = config.Get("provider");
                switch (providerString)
                {
                    case "sqlserver":
                        provider = SqlServerDialect.Provider;
                        break;
                    default :
                        break;
                }
            }
            var dbFactory = new OrmLiteConnectionFactory(connString, provider);
           
            return dbFactory.OpenDbConnection();
        }

        internal static string GenerateConnectionString(string postgreUrl)
        {
            var uriString = postgreUrl;
            var uri = new Uri(uriString);
            var db = uri.AbsolutePath.Trim('/');
            var user = uri.UserInfo.Split(':')[0];
            var passwd = uri.UserInfo.Split(':')[1];
            var port = uri.Port > 0 ? uri.Port : 5432;
            var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
                uri.Host, db, user, passwd, port);

            return connStr;
        }

        public static TimeZoneInfo koreaTZI = TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time");
    }
}
