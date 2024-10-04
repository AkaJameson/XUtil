using DotNet.Util.Core.EasyTcp.Model;
using DotNet.Util.Core.EasyTcp.Tool;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DotNet.Util.Core.EasyTcp
{
    /// <summary>
    /// TCP服务端基类，维护Client列表，服务保活，单点处理，广播等功能
    /// </summary>
    public class TCPServer : IServerToolKit
    {
        public static List<TCPClientModel> clientList = new List<TCPClientModel>();
        private static object listLock = new object();
        public List<Action<byte[],string>> MsgHandler;
        private bool _isStart = false;
        private TcpListener _listener;
        public TCPServer(SocketConfig config, List<Action<byte[],string>> MsgHandler = null)
        {
            if (!IPAddress.TryParse(config.ipAddress, out var address))
            {
                throw new ArgumentException("IpAddress is not Correct");
            }
            this.MsgHandler = MsgHandler ?? new List<Action<byte[],string>>();
            _listener = new TcpListener(address, config.port);
            _isStart = true;
            _listener.Start(config.blockNum);
            //开始接收连接
            _listener.BeginAcceptTcpClient(AcceptConnection, this);
            //连接检测
            MonitorClientConnectionsAsync();
        }
        public TCPServer()
        {
            SocketConfig config = new SocketConfig();
            _listener = new TcpListener(IPAddress.Parse(config.ipAddress), config.port);
            _isStart = true;
            _listener.Start(config.blockNum);
            //开始接收连接
            _listener.BeginAcceptTcpClient(AcceptConnection, this);
            //连接检测
            MonitorClientConnectionsAsync();
        }

        private void AcceptConnection(IAsyncResult asyncResult)
        {
            try
            {
                var client = _listener.EndAcceptTcpClient(asyncResult);
                TCPClientModel model = new TCPClientModel()
                {
                    RemoteIpAddress = client.Client.RemoteEndPoint.ToString(),
                    TcpClient = client,
                    ticks = DateTime.UtcNow.Ticks,
                    Connected = true,
                    safeNetworkStream = new SafeNetworkStream(client.GetStream())
                };
                if (!clientList.Any(p => p.RemoteIpAddress == model.RemoteIpAddress))
                {
                    lock (listLock)
                    {
                        clientList.Add(model);
                    }
                    _ = ProcessClientAsync(model);
                }
                _listener.BeginAcceptTcpClient(AcceptConnection, this);
                Console.WriteLine($"Client {model.RemoteIpAddress} is Connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        private async Task ProcessClientAsync(TCPClientModel clientModel)
        {
            TcpClient client = clientModel.TcpClient;
            SafeNetworkStream safeNetworkStream = clientModel.safeNetworkStream;
            try
            {
                byte[] lengthBuffer = new byte[4]; 
                while (clientModel.Connected)
                {
                    int bytesRead = await safeNetworkStream.ReadAsync(lengthBuffer, 0, 4);
                    if (bytesRead == 0)
                    {
                        Console.WriteLine($"Client {clientModel.RemoteIpAddress} disconnected.");
                        break;
                    }
                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                    if (messageLength <= 0)
                    {
                        Console.WriteLine("Received an invalid message length.");
                        continue;
                    }
                    // 为消息体分配足够的空间
                    byte[] messageBuffer = new byte[messageLength];

                    // 确保读取完整的消息
                    int totalBytesRead = 0;
                    while (totalBytesRead < messageLength)
                    {
                        int remainingBytes = messageLength - totalBytesRead;
                        bytesRead = await safeNetworkStream.ReadAsync(messageBuffer, totalBytesRead, remainingBytes);
                        if (bytesRead == 0)
                        {
                            Console.WriteLine($"Client {clientModel.RemoteIpAddress} disconnected while reading a message.");
                            clientModel.Connected = false;
                            break;
                        }
                        totalBytesRead += bytesRead;
                    }
                    if (totalBytesRead != messageLength)
                    {
                        Console.WriteLine("Failed to read the full message.");
                        continue;
                    }
                    string messageContent = Encoding.UTF8.GetString(messageBuffer);
                    if (messageContent.Equals("HEARTBEAT"))
                    {
                        clientModel.ticks = DateTime.UtcNow.Ticks; 
                        Console.WriteLine($"{clientModel.RemoteIpAddress}: HEARTBEAT");
                        continue;
                    }
                    // 调用消息处理器来处理接收到的消息
                    if (MsgHandler.Count > 0)
                    {
                        foreach (var msg in MsgHandler)
                        {
                            msg.Invoke(messageBuffer, clientModel.RemoteIpAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client {clientModel.RemoteIpAddress}: {ex.Message}");
            }
            finally
            {
                // 清理资源
                safeNetworkStream.Close();
                client.Close();
                lock (listLock)
                {
                    clientList.Remove(clientModel);
                }
                Console.WriteLine($"Client {clientModel.RemoteIpAddress} removed from client list.");
            }
        }

        private bool IsClientConnected(TcpClient client)
        {
            try
            {
                if (client != null && client.Client != null && client.Client.Connected)
                {
                    if (client.Client.Poll(0, SelectMode.SelectRead))
                    {
                        if (client.Client.Available == 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (SocketException)
            {
                return false;
            }
        }
        private void MonitorClientConnectionsAsync()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var clientModel in clientList)
                    {
                        DateTime utcTime = new DateTime(clientModel.ticks, DateTimeKind.Utc);
                        //如果连接时间和当前时间相差一小时以上
                        if ((DateTime.UtcNow - utcTime) > TimeSpan.FromHours(1))
                            clientModel.Connected = false;
                        clientModel.Connected = IsClientConnected(clientModel.TcpClient);

                    }
                    //20分钟一检测
                    await Task.Delay(TimeSpan.FromMinutes(20));
                }
            });

        }

        public void Close()
        {
            try
            {
                if (_isStart)
                {
                    _listener.Stop();
                    _isStart = false;
                    Console.WriteLine("TCP Server stopped listening for new connections.");
                    foreach (var clientModel in clientList)
                    {
                        try
                        {
                            clientModel.safeNetworkStream.Close();
                            clientModel.TcpClient.Close();
                            Console.WriteLine($"Client {clientModel.RemoteIpAddress} disconnected.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error while disconnecting client {clientModel.RemoteIpAddress}: {ex.Message}");
                        }
                    }
                    clientList.Clear();
                    Console.WriteLine("All clients disconnected and cleared.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while closing server: {ex.Message}");
            }
            finally
            {
                _listener.Stop();
                Console.WriteLine("TCP Server resources have been released.");
            }
        }

        private void Start()
        {
            try
            {
                if(!_isStart)
                {
                    _listener.Start();
                    _isStart=true;
                    //开始接收连接
                    _listener.BeginAcceptTcpClient(AcceptConnection, this);
                    //连接检测
                    MonitorClientConnectionsAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TCP Server start error:{ex.Message}");
            }
        }
        /// <summary>
        /// 获取服务端工具包
        /// </summary>
        /// <returns></returns>
        public IServerToolKit GetServerToolKit()
        {
            return this;
        }
        #region ToolKit实现
        /// <summary>
        /// 单点发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="Ipaddress"></param>
        /// <returns></returns>
        public async Task SendMsg(byte[] msg, string Ipaddress)
        {
            var clientModel = clientList.FirstOrDefault(p => p.RemoteIpAddress == Ipaddress);
            if (clientModel is null)
                return;
            try
            {
                byte[] prefixLength = BitConverter.GetBytes(msg.Length);
                await clientModel.safeNetworkStream.WriteAsync(prefixLength, 0, prefixLength.Length);
                await clientModel.safeNetworkStream.WriteAsync(msg, 0, msg.Length);
            }
            catch (SocketException)
            {
                clientModel.Connected = false;
            }
        }
        /// <summary>
        /// 广播发送,线程安全
        /// </summary>
        /// <param name="msg"></param>
        public async Task Boradcast(List<byte[]> msg)
        {
            if (msg.Count != 0)
            {
                foreach (var clientModel in clientList)
                {
                    foreach (var msgModel in msg)
                    {
                        try
                        {
                            byte[] prefixLength = BitConverter.GetBytes(msgModel.Length);
                            await clientModel.safeNetworkStream.WriteAsync(prefixLength, 0, prefixLength.Length);
                            await clientModel.safeNetworkStream.WriteAsync(msgModel, 0, msgModel.Length);
                        }
                        catch (SocketException e)
                        {
                            clientModel.Connected = false;
                            Console.WriteLine(e.ToString());
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取Client模型
        /// </summary>
        /// <param name="Ipaddress"></param>
        /// <returns></returns>
        public TCPClientModel GetClientModel(string Ipaddress) => clientList.FirstOrDefault(p=>p.RemoteIpAddress == Ipaddress);
        /// <summary>
        /// 添加处理方法
        /// </summary>
        /// <param name="handler"></param>
        public void AddMessageHandler(Action<byte[], string> handler)
        {
            if (MsgHandler == null)
                MsgHandler = new();
            MsgHandler.Add(handler);
        }
        #endregion
    }
}
