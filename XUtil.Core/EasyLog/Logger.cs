namespace XUtil.Core.Log
{
    internal class Logger : ILogger
    {
        private static Lazy<Logger> instance;
        private FileWriter Writer;
        private LogConfig config;
        private static bool isInit = false;
        public bool isStart = false;

        Logger(LogConfig logConfig)
        {
            config = logConfig;
        }

        /// <summary>
        /// 获取instance实例
        /// </summary>
        /// <returns></returns>
        public static Logger GetLoggerInstance(LogConfig config)
        {
            if (instance == null)
            {
                instance = new Lazy<Logger>(() => new Logger(config));
            }
            isInit = true;
            return instance.Value;
        }
        public static void SetLoggerInstance(LogConfig config)
        {
            if(instance == null)
            {
                instance = new Lazy<Logger>(() => new Logger(config));
                isInit = true;
            }
        }
        public static Logger GetLoggerInstance()
        {
            if (isInit)
                return instance.Value;
            else
                throw new Exception("Logger 未初始化");
        }
        public void Info(string message)
        {
            if (isStart)
                WriteLog(LogLevel.Info, message);
        }

        public void Warn(string message)
        {
            if (isStart)
                WriteLog(LogLevel.Warning, message);
        }
        public void Error(string message)
        {
            if (isStart)
                WriteLog(LogLevel.Error, message);
        }
        private void WriteLog(LogLevel level, string message)
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logmessage = string.Concat($"{config.Prefix}::{currentTime}::", level.ToString(), "-----", message);
            Writer.SetQueue(logmessage);
        }

        public void LoggerStart()
        {
            if(Writer == null)
            {
                Writer = new FileWriter(config);
                Writer.Start();
                isStart = true;
            }
        }
        public async void LoggerStop()
        {
            Writer = null;
            isStart = false;
        }
    }
}
