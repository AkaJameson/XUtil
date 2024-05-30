using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DB.DbLog
{
    public interface ILogSaver
    {
        void WriteSuccessLog(string log);
        void WriteWarningLog(string log);
        void WriteErrorLog(string log);
        void WriteExceptionLog(string log);
    }
}
