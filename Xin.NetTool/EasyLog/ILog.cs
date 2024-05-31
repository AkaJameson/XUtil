using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.NetTool.EasyLog
{
    internal interface ILog
    {
        void LogSuccess(string log);
        void LogWarning(string log);
        void LogError(string log);
        void LogException(string log);
    }
}
