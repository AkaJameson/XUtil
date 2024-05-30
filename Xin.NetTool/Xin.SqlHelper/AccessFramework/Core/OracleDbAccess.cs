using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xin.DB.AccessFramework.Interface;

namespace Xin.DB.AccessFramework.Core
{
    public class OracleDbAccess : IDbAccess
    {
        public Xin.DB.AccessFramework.Core.DbType DbType { get; set; }
        public IDbo Dbo { get; set; }
        public DbConnection Connection { get; set; }

        public OracleDbAccess(DbType dbType,IDbo dbo,string connectionString)
        {
            this.DbType = dbType;
            this.Dbo = dbo;
            Connection = new OracleConnection(connectionString);
            Connection.Open();

            dbo.connection = Connection;
        }

        public void Dispose(bool isDispose)
        {
            if(isDispose)
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
