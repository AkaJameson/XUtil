using System.Net.Sockets;

namespace DotNet.Util.Core.EasyTcp.Model
{
    public class TCPClientModel
    {
        /// <summary>
        ///  远端ip地址
        /// </summary>
        public string RemoteIpAddress { get; set; }
        /// <summary>
        /// TCP客户端
        /// </summary>
        public TcpClient TcpClient { get; set; }
        /// <summary>
        /// 连接时间
        /// </summary>
        public long ticks { get; set; }
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Connected { get; set; } = false;
        /// <summary>
        /// 流
        /// </summary>
        public SafeNetworkStream safeNetworkStream { get; set; }
    }
}
