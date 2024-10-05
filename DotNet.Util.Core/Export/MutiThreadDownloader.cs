using System.Net.Http.Headers;

namespace XUtil.Core.Export
{
    /// <summary>
    /// 多线程下载器支持断点续传和进度保存
    /// </summary>
    public class MultiThreadDownloader
    {
        public static HttpClient client = new HttpClient();
        private string progressFileName;
        /// <summary>
        /// 保存路径
        /// </summary>
        /// <param name="progresssavePath"> 保存路径，不使用文件名 </param>
        /// <exception cref="Exception"></exception>
        public MultiThreadDownloader(string progresssavePath)
        {
            if(!Directory.Exists(progresssavePath))
            {
                throw new Exception("保存路径不存在");
            }
            else
            {
                progressFileName =Path.Combine(progresssavePath, DateTime.UtcNow.Ticks+ "progressFile.txt");

            }
        }


        public async Task DownloadFilesAsync(List<string> urls, string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
            {
                throw new Exception("目标文件夹不存在");
            }
            Dictionary<string, long> progress = LoadProgress();
            List<Task> downloadTasks = new List<Task>();
            OnDownloadStartHandler?.Invoke();
            foreach (var url in urls)
            {
                lock (downloadTasks)
                {
                    downloadTasks.Add(DownLoadFileAsync(url, destinationFolder, progress));
                }
            }

            await Task.WhenAll(downloadTasks);
            OnDownloadDoneHandler?.Invoke();
        }

        private async Task DownLoadFileAsync(string url, string destinationFolder, Dictionary<string, long> progress)
        {
            Uri uri = new Uri(url);
            string fileName = Path.GetFileName(uri.LocalPath);
            string destinationPath = Path.Combine(destinationFolder, fileName);
            long existingLength = progress.ContainsKey(url) == true ? progress[url] : 0;

            try
            {
                using(HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    request.Headers.Range = new RangeHeaderValue(existingLength, null);
                    using(HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        using (Stream stream = response.Content.ReadAsStream(),filestream = new FileStream(destinationPath, FileMode.Append, FileAccess.Write, FileShare.None, 8192, true))
                        {
                            byte[] buffer = new byte[8192];
                            long readByte = 0;
                            long totalReadBytes = existingLength;
                            while((readByte = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await filestream.WriteAsync(buffer, 0, (int)readByte);
                                totalReadBytes += readByte;
                                SaveProgress(url, totalReadBytes);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"下载失败{url}:{e.Message}");
            }
        }

        /// <summary>
        /// 加载进度文件
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Dictionary<string,long> LoadProgress()
        {
            Dictionary<string,long> progress = new Dictionary<string, long>();

            if (File.Exists(progressFileName))
            {
                string[] lines = File.ReadAllLines(progressFileName);
                foreach(var line in lines)
                {
                    string[] parts = line.Split(',');
                    progress.Add(parts[0], long.Parse(parts[1]));
                }
            }
                return progress;
        }

        /// <summary>
        /// 保存下载进度
        /// </summary>
        /// <param name="url"></param>
        /// <param name="byteDownloaded"></param>
        private void SaveProgress(string url,long byteDownloaded)
        {
            string[] lines = File.Exists(progressFileName)? File.ReadAllLines(progressFileName) : new string[0];
            bool update = false;
            for(int count = 0; count < lines.Length; count++)
            {
                string[] parts = lines[count].Split(',');
                if (parts[0] == url)
                {
                    parts[1] = byteDownloaded.ToString();
                    lines[count] = string.Join(",", parts);
                    update = true;
                    break;
                }
            }
            if (!update)
            {
                List<string> newlines = new List<string>(lines) { $"{url},{byteDownloaded}" };
                lines = newlines.ToArray();
            }

            File.WriteAllLines(progressFileName, lines);
        }
        /// <summary>
        /// 下载完成事件
        /// </summary>
        Action OnDownloadDoneHandler;
        /// <summary>
        /// 开始下载事件
        /// </summary>
        Action OnDownloadStartHandler;
        
    }
}
