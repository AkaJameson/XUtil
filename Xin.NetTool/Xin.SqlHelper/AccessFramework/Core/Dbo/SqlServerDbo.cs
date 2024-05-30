﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xin.DB.AccessFramework.Interface;

namespace Xin.DB.AccessFramework.Core.Dbo
{
    internal class SqlServerDbo : IDbo
    {
        public DbConnection connection;
        public SqlServerDbo(DbConnection connection)
        {
            this.connection = connection;
        }

        DbConnection IDbo.connection { get; set; }

        public DbParameter CreateDbParameter(string parameterName, System.Data.DbType dbtype, object value)
        {
            throw new NotImplementedException();
        }

        public DbParameter CreateOutPutParameter(string parameterName, System.Data.DbType dbtype, object value)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSet(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTable(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public DbDataReader ExecuteReader(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public bool ExecuteTransaction(List<DbCommand> dbCommands)
        {
            throw new NotImplementedException();
        }
    }
}
