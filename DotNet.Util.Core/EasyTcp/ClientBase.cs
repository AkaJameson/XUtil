using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Core.EasyTcp
{
    public abstract class ClientBase
    {
        private TcpClient _client;
        private bool isConnect = false;
        private bool isDispose = false;
        private NetworkStream _stream;
        private List<Action<string>> OnReceiveActions = new();
        private byte[] buffer;
        private int intervalMillSecond = 10;
        private Guid client_identity;
        private object _startLocker = new object();
        private readonly object _streamLock = new object();
        private int Retry = 0;
        protected ClientBase()
        {
            _client = new TcpClient();
            client_identity = Guid.NewGuid();
        }

        public async void Start(IPEndPoint IpEndPoint, List<Action<string>>? OnReceiveMessageAction, Int32 bufferSize = 1024)
        {
            lock (_startLocker)
            {
                if (!isConnect)
                {
                    buffer = new byte[bufferSize];
                    _client.Connect(IpEndPoint);
                    _stream = _client.GetStream();
                    _stream.Write(client_identity.ToByteArray(), 0, client_identity.ToByteArray().Length);
                    _stream.Flush();
                    KeepAlive();
                    _stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCallBack), null);
                    _stream.Position = 0;
                    if (OnReceiveMessageAction != null)
                        OnReceiveActions = OnReceiveMessageAction;
                    isConnect = true;
                    isDispose = false;
                }
            }
        }

        public void Stop()
        {
            lock (_startLocker) // 确保线程安全
            {
                if (isConnect)
                {
                    try
                    {
                        _stream?.Close();
                        _stream?.Dispose();
                        _client?.Close();
                        _client?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during stopping: {ex.Message}");
                    }
                    finally
                    {
                        OnReceiveActions.Clear();
                        isConnect = false;
                        isDispose = true; 
                    }
                }
            }
        }

        public void KeepAlive()
        {
            Task.Run(async () =>
            {
                while (isConnect && !isDispose)
                {
                    if (!isConnect && isDispose)
                        break;
                    lock (_streamLock)
                    {
                        try
                        {
                            byte[] heartBeatMessage = Encoding.UTF8.GetBytes($"HEARTBEAT:{client_identity.ToByteArray()}");
                            _stream.Write(heartBeatMessage, 0, heartBeatMessage.Length);
                            _stream.Flush();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to send heartbeat: {ex.Message}");
                            Retry++;
                            if (Retry > 3)
                            {
                                isConnect = false;
                                break;
                            }
                        }
                    }
                    await Task.Delay(intervalMillSecond);
                }
            });
        }


        public void ReadCallBack(IAsyncResult ar)
        {
            if (!isConnect)
            {
                Stop();
                return;
            }
           int readlength =  _stream.EndRead(ar);
            
            if(readlength > 0)
            {
                string data = Encoding.UTF8.GetString(buffer, 0, readlength);
                foreach(var action in OnReceiveActions)
                {
                    action?.Invoke(data);
                }
            }
            _stream.BeginRead(buffer,0,buffer.Length,new AsyncCallback(ReadCallBack),null);
        }

        public void Send(string message, Encoding encoding)
        {
            if (!isConnect)
            {
                throw new InvalidOperationException("Client is not connected.");
            }

            byte[] data = encoding.GetBytes(message);
            Send(data);
        }

        public void Send(byte[] data)
        {
            lock (_streamLock)
            {
                if (!isConnect)
                {
                    throw new InvalidOperationException("Client is not connected.");
                }

                try
                {
                    _stream.Write(data, 0, data.Length);
                    _stream.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send data: {ex.Message}");
                    Stop(); // 在发送失败时可以选择停止连接
                }
            }
        }


    }
}
