using System.Net.Sockets;
namespace XUtil.Core.EasyTcp
{
    /// <summary>
    /// 线程安全的访问流
    /// </summary>
    public class SafeNetworkStream
    {
        private readonly NetworkStream _networkStream;
        private readonly SemaphoreSlim _readSemaphore;
        private readonly SemaphoreSlim _writeSemaphore;

        public SafeNetworkStream(NetworkStream networkStream)
        {
            _networkStream = networkStream ?? throw new ArgumentNullException(nameof(networkStream));
            _readSemaphore = new SemaphoreSlim(1, 1);  
            _writeSemaphore = new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// 异步读取数据
        /// </summary>
        /// <param name="buffer">用于存放读取数据的缓冲区</param>
        /// <param name="offset">在缓冲区中的起始索引</param>
        /// <param name="size">要读取的最大字节数</param>
        /// <param name="cancellationToken">用于取消操作的标记</param>
        /// <returns>实际读取的字节数</returns>
        public async Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken = default)
        {
            await _readSemaphore.WaitAsync(cancellationToken);
            try
            {
                // 从 NetworkStream 读取数据
                return await _networkStream.ReadAsync(buffer, offset, size, cancellationToken);
            }
            finally
            {
                _readSemaphore.Release();
            }
        }

        /// <summary>
        /// 异步写入数据
        /// </summary>
        /// <param name="buffer">要写入的数据</param>
        /// <param name="offset">在数据中的起始索引</param>
        /// <param name="size">要写入的字节数</param>
        /// <param name="cancellationToken">用于取消操作的标记</param>
        public async Task WriteAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken = default)
        {
            await _writeSemaphore.WaitAsync(cancellationToken);
            try
            {
                // 将数据写入 NetworkStream
                await _networkStream.WriteAsync(buffer, offset, size, cancellationToken);
                await _networkStream.FlushAsync(cancellationToken);
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        public void Close()
        {
            _networkStream.Dispose();
            _networkStream.Close();
        }

        /// <summary>
        /// 获取底层 NetworkStream
        /// </summary>
        /// <returns>返回底层 NetworkStream</returns>
        public NetworkStream GetUnderlyingStream()
        {
            return _networkStream;
        }
    }

}
