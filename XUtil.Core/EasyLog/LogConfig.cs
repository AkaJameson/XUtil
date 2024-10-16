﻿using System.Reflection;

namespace XUtil.Core.Log
{
    public struct LogConfig
    {
        public LogConfig()
        {
        }
        public string BasePath { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string LogFile { get; set; } = "MyLog";
        public string Prefix { get; set; } = string.Empty;
    }

    public enum LogLevel
    {
        Info,
        Error,
        Warning
    }
}
