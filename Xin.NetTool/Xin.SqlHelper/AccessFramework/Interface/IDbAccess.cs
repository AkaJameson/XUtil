using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DB.AccessFramework.Interface
{
    public interface IDbAccess:IDisposable
    {
        public Xin.DB.AccessFramework.Core.DbType DbType { get; set; }
        public IDbo Dbo { get; set; }
        DbConnection Connection { get; set; }
        void Dispose(bool isDispose);

       
    }
}
