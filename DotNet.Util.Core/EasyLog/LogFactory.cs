using System.Security.Cryptography.X509Certificates;

namespace Xin.DotnetUtil.Log
{
    public static class LogFactory
    { 
        private static object locker = new object();   

        public static ILogger CreateLogger(LogConfig logConfig)
        {
            try
            {
                lock (locker)
                {
                    var logger = Logger.GetLoggerInstance(logConfig);
                    logger.LoggerStart();
                    return logger;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static ILogger CreateLogger()
        {
            try
            {
                lock (locker)
                {
                    var logger = Logger.GetLoggerInstance();
                    return logger;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }
        public static void SetLogger(LogConfig logConfig)
        {
            try
            {
                lock (locker)
                {
                    Logger.SetLoggerInstance(logConfig);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public static void StopLogger(ILogger logger)
        {
            lock (locker)
            {
                if (logger != null)
                {
                    Logger log = (Logger)logger;
                    log.LoggerStop();
                }
            }
        }

        public static void StartLogger(ILogger logger)
        {
            lock (locker)
            {
                if (logger != null)
                {
                    Logger log = (Logger)logger;
                    log.LoggerStop();
                }
            }
        }

    }
}
