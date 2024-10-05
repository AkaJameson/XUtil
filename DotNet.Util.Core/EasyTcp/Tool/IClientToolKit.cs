using XUtil.Core.EasyTcp.Model;
namespace XUtil.Core.EasyTcp.Tool
{
    public interface IClientToolKit
    {
        Task SendMsg(byte[] msg);
        TCPClientModel GetTcpClient();
        void AddMessageHandler(Action<byte[]> handler);

    }
}
