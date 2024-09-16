using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.Log
{
    public struct LogConfig
    {
        public LogConfig()
        {

        }
        public string BasePath { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string LogFile { get; set; } = "MyLog";
        public string Prefix { get; set; } = string.Empty;
        /// <summary>
        /// 单位：秒
        /// </summary>
        public int SleepTime { get; set; } = 5;
    }

    public enum LogLevel
    {
        Info,
        Error,
        Warning
    }
}
