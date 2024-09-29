using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Core.EasyTcp
{
    public abstract class ServerBase
    {
        private TcpListener _listener;
        private ConcurrentDictionary<Guid, Tuple<int, TcpClient>> Client;
        private bool _isStart = false;
        private object _startLocker = new object();
        private readonly object _streamLock = new object();
        protected ServerBase(IPAddress iPAddress,int blockNum,int port)
        {
            _listener = new TcpListener(iPAddress, port);
        }


        public void Start()
        {
            lock(_startLocker)
            {
                if (!_isStart)
                {
                    
                }
            }

        }
    }
}
