using DotNet.Util.Core.EasyTcp.Model;
using DotNet.Util.Core.EasyTcp.Tool;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DotNet.Util.Core.EasyTcp
{
    public class TCPClientBase : IClientToolKit
    {
        private TCPClientModel TcpClientModel { get; set; } = new TCPClientModel();
        private List<Action<byte[]>> msgHandler;
        private IPEndPoint IPEndPoint { get; set; }
        public TCPClientBase(IPEndPoint iPEndPoint, List<Action<byte[]>> msgHandler = null)
        {
            try
            {
                TcpClientModel.TcpClient = new TcpClient();
                IPEndPoint = iPEndPoint;
                TcpClientModel.TcpClient.Connect(iPEndPoint);
                TcpClientModel.safeNetworkStream = new SafeNetworkStream(TcpClientModel.TcpClient.GetStream());
                TcpClientModel.ticks = DateTime.UtcNow.Ticks;
                TcpClientModel.Connected = true;
                this.msgHandler = msgHandler?? new List<Action<byte[]>>();
                _ = KeepAlive();
                _ = MsgHandle();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private async Task KeepAlive()
        {
            while (TcpClientModel.Connected)
            {
                try
                {

                    await SendMsg(Encoding.UTF8.GetBytes("HEARTBEAT"));
                    TcpClientModel.ticks = DateTime.UtcNow.Ticks;
                    await Task.Delay(TimeSpan.FromSeconds(20));
                }
                catch (Exception ex)
                {
                    await ReconnectAsync();
                    Console.WriteLine(ex.Message);
                }
            };
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        private async Task ReconnectAsync()
        {
            TcpClientModel.Connected = false;
            while (!TcpClientModel.Connected)
            {
                try
                {
                    Console.WriteLine("Attempting to reconnect...");
                    TcpClientModel.TcpClient.Close();
                    TcpClientModel.TcpClient = new TcpClient();
                    await TcpClientModel.TcpClient.ConnectAsync(IPEndPoint.Address, IPEndPoint.Port);
                    TcpClientModel.safeNetworkStream = new SafeNetworkStream(TcpClientModel.TcpClient.GetStream());
                    TcpClientModel.ticks = DateTime.Now.Ticks;
                    TcpClientModel.Connected = true;
                    Console.WriteLine("Reconnection successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnect failed: {ex.Message}. Retrying in 5 seconds...");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            }
        }
        private async Task MsgHandle()
        {
            try
            {
                byte[] lengthBuffer = new byte[4];
                while (TcpClientModel.Connected)
                {
                    int lengthBytesRead = await TcpClientModel.safeNetworkStream.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                    if (lengthBytesRead == 0)
                    {
                        Console.WriteLine("Server disconnected.");
                        TcpClientModel.Connected = false;
                        break;
                    }
                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                    if (messageLength <= 0)
                    {
                        Console.WriteLine("Received an invalid message length.");
                        continue;
                    }
                    byte[] messageBuffer = new byte[messageLength];
                    int totalBytesRead = 0;
                    while (totalBytesRead < messageLength)
                    {
                        int remainingBytes = messageLength - totalBytesRead;
                        int bytesRead = await TcpClientModel.safeNetworkStream.ReadAsync(messageBuffer, totalBytesRead, remainingBytes);

                        if (bytesRead == 0)
                        {
                            Console.WriteLine("Server disconnected while reading message.");
                            TcpClientModel.Connected = false;
                            break;
                        }

                        totalBytesRead += bytesRead;
                    }
                    if (totalBytesRead != messageLength)
                    {
                        Console.WriteLine("Failed to read the full message from the server.");
                        continue;
                    }
                    if (msgHandler.Count != 0)
                    {
                        foreach (var msg in msgHandler)
                        {
                            msg.Invoke(messageBuffer); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TcpClientModel.Connected = false;
            }
        }

        public IClientToolKit GetClientToolKit()
        {
            return this;
        }
        #region 实现ClientToolkit
        public async Task SendMsg(byte[] msg)
        {
            try
            {
                if (TcpClientModel.Connected)
                {
                    byte[] prefixLength = BitConverter.GetBytes(msg.Length);
                    await TcpClientModel.safeNetworkStream.WriteAsync(prefixLength,0, prefixLength.Length);
                    await TcpClientModel.safeNetworkStream.WriteAsync(msg, 0, msg.Length);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TcpClientModel.Connected = false;
            }

        }
        public TCPClientModel GetTcpClient()
        {
            return this.TcpClientModel;
        }

        public void AddMessageHandler(Action<byte[]> handler)
        {
            msgHandler.Add(handler);
        }
        #endregion



    }
}
