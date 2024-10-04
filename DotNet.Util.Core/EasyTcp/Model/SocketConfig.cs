using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Core.EasyTcp.Model
{
    public class SocketConfig
    {
        public int blockNum { get; set; } = 20;
        public string ipAddress { get; set; } = "0.0.0.0";
        public int port { get; set; } = 49280;
    }

}
