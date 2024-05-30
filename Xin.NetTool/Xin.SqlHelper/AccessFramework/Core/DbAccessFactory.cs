using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xin.DB.AccessFramework.Core.Dbo;
using Xin.DB.AccessFramework.Interface;

namespace Xin.DB.AccessFramework.Core
{
    internal class DbAccessFactory
    {
        public static IDbAccess CreateDbAccess(DbType dbType,string connectionString)
        {
            IDbAccess dbAccess = null;
            switch (dbType)
            {
                case DbType.Oracle:
                    dbAccess = CreateOracleAccess(connectionString);
                    break;
                case DbType.Mysql:
                    dbAccess = CreateMysqlAccess(connectionString);
                    break;
                case DbType.SqlServer:
                    dbAccess = CreateSqlServerAccess(connectionString);
                    break;
                case DbType.SqlLite:
                    dbAccess = CreateSqlLiteAccess(connectionString);
                    break;
                default:
                    break;
            }
            return dbAccess;
        }

        private static IDbAccess CreateOracleAccess( string connectionString)
        {
            return new OracleDbAccess(DbType.Mysql, new OracleDbo(), connectionString);
        }

        private static IDbAccess CreateMysqlAccess(string connectionString)
        {

            return new MySqlDbAccess(DbType.Mysql,new MysqlDbo(), connectionString);
        }

        private static IDbAccess CreateSqlLiteAccess(string connectionString)
        {
            return new SqLiteDbAccess(DbType.Oracle,new SqliteDbo(), connectionString);
        }

        private static IDbAccess CreateSqlServerAccess(string connectionString)
        {
            return new SqlServerDbAccess(DbType.SqlServer, new SqlServerDbo(), connectionString);
        }
    }
}
