using DotNet.Util.Core.EasyTcp.Model;

namespace DotNet.Util.Core.EasyTcp.Tool
{
    public interface IClientToolKit
    {
        Task SendMsg(byte[] msg);
        TCPClientModel GetTcpClient();
        void AddMessageHandler(Action<byte[]> handler);

    }
}
