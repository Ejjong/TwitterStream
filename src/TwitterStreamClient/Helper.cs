using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamClient
{
    public static class Helper
    {
        //public static IEnumerable<T> GetList<T>(this IDbConnection connection)
        //{
        //    return connection.GetList<T>(new { });
        //}

        //public static IEnumerable<T> GetList<T>(this IDbConnection connection, object whereConditions)
        //{
        //    var currenttype = typeof(T);
        //    var idProps = GetIdProperties(currenttype).ToList();

        //    if (!idProps.Any())
        //        throw new ArgumentException("Entity must have at least one [Key] property");

        //    var name = GetTableName(currenttype);

        //    var sb = new StringBuilder();
        //    var whereprops = GetAllProperties(whereConditions).ToArray();
        //    sb.AppendFormat("Select * from {0}", name);

        //    if (whereprops.Any())
        //    {
        //        sb.Append(" where ");
        //        BuildWhere(sb, whereprops);
        //    }

        //    if (Debugger.IsAttached)
        //        Trace.WriteLine(String.Format("GetList<{0}>: {1}", currenttype, sb));

        //    return connection.Query<T>(sb.ToString(), whereConditions);
        //}

    }
}
