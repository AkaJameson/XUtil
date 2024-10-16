using System.Collections.Concurrent;

namespace XUtil.Core.Log
{
    internal class FileWriter
    {
        public FileWriter(LogConfig logConfig)
        {

            this.logConfig = logConfig;
            CreateLogFile();
        }
        private LogConfig logConfig;
        private ConcurrentQueue<string> LoggerQueue { get; set; } = new ConcurrentQueue<string>();
        private FileStream fs = null;
        private StreamWriter writer = null;
        private FileInfo fileInfo;
        private bool isWriting = false;
        private void CreateLogFile()
        {
            string directoryPath = Path.Combine(logConfig.BasePath, logConfig.LogFile);
            string filePath = Path.Combine(directoryPath, $"log{DateTime.UtcNow.Ticks}.txt");
            Directory.CreateDirectory(directoryPath);
            fileInfo = new FileInfo(filePath);
            if (!File.Exists(filePath))
                File.Create(filePath).Dispose();
            fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(fs);
        }

        public void SetQueue(string message)
        {
            LoggerQueue.Enqueue(message);
            if(!isWriting)
                QueenWrite();
        }
        /// <summary>
        /// 检查文件大小
        /// </summary>
        /// <returns></returns>
        private async Task CheckFileSize()
        {
            if (fileInfo.Length > 20 * 1024 * 1024)
            {
                await fs.DisposeAsync();
                await writer.DisposeAsync();
                string directoryPath = Path.Combine(logConfig.BasePath, logConfig.LogFile);
                string filePath = Path.Combine(directoryPath, $"log{DateTime.UtcNow.Ticks}.txt");
                Directory.CreateDirectory(directoryPath);
                if (!File.Exists(filePath))
                    File.Create(filePath).Dispose();
                fileInfo = new FileInfo(filePath);
            }
        }
        private async Task QueenWrite()
        {
            while (LoggerQueue.Count > 0)
            {
                isWriting = true;
                try
                {
                    LoggerQueue.TryDequeue(out var logger);
                    if (!string.IsNullOrWhiteSpace(logger))
                    {
                        await writer.WriteLineAsync(logger);
                        await writer.FlushAsync();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    throw e;
                }
            }
            await CheckFileSize();
            isWriting = false;
            
        }
    }
}
