using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DB.AccessFramework.Interface
{
    public interface IDbo
    {
        public DbConnection connection { get; set; }
        int ExecuteNonQuery(string commandText, CommandType commandType, params DbParameter[] parameters);
        object ExecuteScalar(string commandText, CommandType commandType, params DbParameter[] parameters);

        DbDataReader ExecuteReader(string commandText, CommandType commandType, params DbParameter[] parameters);

        DataTable ExecuteDataTable(string commandText, CommandType commandType, params DbParameter[] parameters);

        DataSet ExecuteDataSet(string commandText, CommandType commandType, params DbParameter[] parameters);

        bool ExecuteTransaction(List<DbCommand> dbCommands);

        DbParameter CreateDbParameter(string parameterName, DbType dbtype, object value);

        DbParameter CreateOutPutParameter(string parameterName, DbType dbtype, object value);

    }
}
