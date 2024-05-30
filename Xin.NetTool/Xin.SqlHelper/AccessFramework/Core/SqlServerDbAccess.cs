using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xin.DB.AccessFramework.Interface;

namespace Xin.DB.AccessFramework.Core
{
    public class SqlServerDbAccess : IDbAccess
    {
        public Xin.DB.AccessFramework.Core.DbType DbType { get; set; }
        public IDbo Dbo { get; set; }
        public DbConnection Connection { get; set; }

        public SqlServerDbAccess(DbType dbType,IDbo dbo,string ConnectionString)
        {
            this.DbType = dbType;
            this.Dbo = dbo;
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();

            dbo.connection = Connection;
        }

        public void Dispose(bool isDispose)
        {
            if (isDispose)
            {
                if(Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
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
