using System.Collections.Concurrent;

namespace XUtil.Core.Log
{
    internal class FileWriter
    {
        public FileWriter(LogConfig logConfig)
        {

            this.logConfig = logConfig;
            this.filePath =CreateLogFile();
        }
        private LogConfig logConfig;
        private ConcurrentQueue<string> LoggerQueue { get; set; } = new ConcurrentQueue<string>();
        public int GetPreLogCount()=> LoggerQueue.Count();
        public string filePath;
        private FileInfo fileInfo;
        private string CreateLogFile()
        {
            string directoryPath = Path.Combine(logConfig.BasePath, logConfig.LogFile);
            string filePath = Path.Combine(directoryPath, $"log{DateTime.UtcNow.Ticks}.txt");
            Directory.CreateDirectory(directoryPath);
            if (!File.Exists(filePath))
                File.Create(filePath).Dispose();
            fileInfo = new FileInfo(filePath);
            return filePath;
        }

        //开始循环写入文件，并在队列为空的时候sleep，保证单线程操作
        public void Start()
        {
            FileStream fs = null;
            StreamWriter writer = null; 

            Task.Run(async () =>
            {
                fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                writer = new StreamWriter(fs);
                while (true)
                {
                    try
                    {
                        if(fileInfo.Length > 20 * 1024 * 1024)
                        {
                            await fs.DisposeAsync();
                            await writer.DisposeAsync();
                            string directoryPath = Path.Combine(logConfig.BasePath, logConfig.LogFile);
                            string filePath = Path.Combine(directoryPath, $"log{DateTime.UtcNow.Ticks}.txt");
                            Directory.CreateDirectory(directoryPath);
                            if (!File.Exists(filePath))
                                File.Create(filePath).Dispose();
                            fileInfo = new FileInfo(filePath);
                            this.filePath = filePath;
                            fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                            writer = new StreamWriter(fs);
                        }
                        if (LoggerQueue.Count > 0)
                        {
                            LoggerQueue.TryDequeue(out var logger);
                            if (!string.IsNullOrWhiteSpace(logger))
                            {
                                await writer.WriteLineAsync(logger);
                                await writer.FlushAsync();
                            }

                        }
                        else
                            await Task.Delay(logConfig.SleepTime * 1000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        throw e;
                    }
                }
            });
        }
        public void SetQueue(string message)
        {
            //防止日志队列数据量过大，做一些处理
            if(LoggerQueue.Count > 200000)
            {
                return;
            }
            LoggerQueue.Enqueue(message);
        }
    }
}
