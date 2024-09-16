using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.Log
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
        private string CreateLogFile()
        {
            string directoryPath = Path.Combine(logConfig.BasePath, logConfig.LogFile);
            string filePath = Path.Combine(directoryPath, "log.txt");
            Directory.CreateDirectory(directoryPath);
            if (!File.Exists(filePath))
                File.Create(filePath).Dispose();
            return filePath;
        }

        //开始循环写入文件，并在队列为空的时候sleep，保证单线程操作
        public void Start()
        {
            Task.Run(async () =>
            {
                await using var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                await using var writer = new StreamWriter(fs);
                while (true)
                {
                    try
                    {
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
            //防止日志数据量过大，做一些简陋的处理
            if(LoggerQueue.Count > 200000)
            {
                return;
            }
            LoggerQueue.Enqueue(message);
        }
    }
}
