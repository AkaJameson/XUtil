using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DotNet.Util.Core.EasyTcp.Model;

namespace DotNet.Util.Core.EasyTcp.Tool
{
    public interface IServerToolKit
    {
        /// <summary>
        /// 单点发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ipaddress"></param>
        /// <returns></returns>
        Task SendMsg(byte[] msg, string Ipaddress);
        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task Boradcast(List<byte[]> msg);

        TCPClientModel GetClientModel(string Ipaddress);
        /// <summary>
        /// 添加服务器处理方法
        /// </summary>
        /// <param name="handler"></param>
        void AddMessageHandler(Action<byte[], string> handler);
    }
}
