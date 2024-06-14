using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.EasyLog
{
    internal interface ILog
    {
        void LogSuccess(string log);
        void LogWarning(string log);
        void LogError(string log);
        void LogException(string log);

        void LogSuccessAsync(string log);
        void LogWarningAsync(string log);
        void LogErrorAsync(string log);
        void LogExceptionAsync(string log);
    }
}
