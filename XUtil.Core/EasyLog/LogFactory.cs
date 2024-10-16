using System.Security.Cryptography.X509Certificates;

namespace XUtil.Core.Log
{
    public static class LogFactory
    { 
        private static object locker = new object();   

        public static ILogger CreateLogger(LogConfig logConfig)
        {
            lock (locker)
            {
                var logger = Logger.GetLoggerInstance(logConfig);
                logger.StartLogger();
                return logger;
            }
        }
        public static ILogger GetLogger()
        {
            lock (locker)
            {
                return Logger.GetLoggerInstance();
            }
        }
        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="logger"></param>
        public static void Unload(ILogger logger)
        {
            lock (locker)
            {
                if (logger != null)
                {
                    ((Logger)logger).StopLogger();
                }
            }
        }
    }
}
