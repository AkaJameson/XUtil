using System.Net.Sockets;
using System.Net;

namespace DotNet.Util.Core.EasyUdp
{
    public class UDPHelper
    {
        private UdpClient udpClient;
        private IPEndPoint remoteEndPoint;
        private bool isListening;
        public UDPHelper(int port)
        {
            udpClient = new UdpClient(port);
            isListening = false;
        }

        public UDPHelper(string remoteAddress, int remotePort)
        {
            udpClient = new UdpClient();
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteAddress), remotePort);
        }
        public void Send(byte[] message)
        {
            if (remoteEndPoint == null)
                throw new InvalidOperationException("Remote end point is not set.");
            udpClient.Send(message, message.Length, remoteEndPoint);
        }
        /// <summary>
        /// 单独接收消息
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ReceiveAsync(IPEndPoint iPEndPoint)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 0);
            var data = await udpClient.ReceiveAsync();
            iPEndPoint = data.RemoteEndPoint;
            return data.Buffer;
        }
        /// <summary>
        /// 开启监听
        /// </summary>
        /// <param name="onMessageReceived"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void StartListeningAsync(List<Action<byte[], IPEndPoint>> onMessageReceived)
        {
            if (isListening)
                throw new InvalidOperationException("Already listening.");

            isListening = true;

            Task.Run(async () =>
            {
                while (isListening)
                {
                    try
                    {
                        var result = await udpClient.ReceiveAsync();
                        foreach (var item in onMessageReceived)
                        {
                            item.Invoke(result.Buffer, result.RemoteEndPoint);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error receiving UDP message: {ex.Message}");
                    }
                }
            });
        }

        // 停止监听
        public void StopListening()
        {
            isListening = false;
        }

        // 关闭UDP连接
        public void Close()
        {
            StopListening();
            udpClient.Close();
        }
    }
}
