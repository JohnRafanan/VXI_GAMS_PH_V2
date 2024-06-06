using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace VXI_GAMS_US.HELPER
{
    public static class DbContextExtensions
    {
        /*
         * need
            Only EntityFramework
         */
        public static DataTable DataTable(this DbContext context, string sqlQuery,
            params DbParameter[] parameters)
        {
            var dataTable = new DataTable();
            var connection = context.Database.Connection;
            var dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                if (cmd == null)
                    throw new Exception("Cannot create query command");
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                    foreach (var item in parameters)
                        cmd.Parameters.Add(item);
                using (var adapter = dbFactory.CreateDataAdapter())
                {
                    if (adapter == null)
                        throw new Exception("Cannot create query command");
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public static DataSet DataSet(this DbContext context, string sqlQuery,
            params DbParameter[] parameters)
        {
            var dataSet = new DataSet();
            var connection = context.Database.Connection;
            var dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                if (cmd == null)
                    throw new Exception("Cannot create query command");
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                    foreach (var item in parameters)
                        cmd.Parameters.Add(item);
                using (var adapter = dbFactory.CreateDataAdapter())
                {
                    if (adapter == null)
                        throw new Exception("Cannot create query command");
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataSet);
                }
            }
            return dataSet;
        }
    }
}
