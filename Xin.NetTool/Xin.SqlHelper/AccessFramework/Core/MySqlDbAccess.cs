using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xin.DB.AccessFramework.Interface;

namespace Xin.DB.AccessFramework.Core
{
    public class MySqlDbAccess : IDbAccess
    {
        public Xin.DB.AccessFramework.Core.DbType DbType { get; set; }
        public IDbo Dbo { get; set; }
        DbConnection connection { get; set; }
        public DbConnection Connection { get; set; }

        public MySqlDbAccess(DbType dbType,IDbo dbo,string connectingString)
        {
            this.DbType = dbType;

            this.Dbo = dbo;

            Connection = new MySqlConnection(connectingString);

            Connection.Open();

            dbo.connection = Connection;
        }

        /// <summary>
        /// 实现接口连接数据库
        /// </summary>
        /// <param name="isDispose"></param>
        public void Dispose(bool isDispose)
        {
            if (isDispose)
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
