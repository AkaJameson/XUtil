using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
namespace Xin.NetTool.SysInfo
{
    public class NetInfo
    {
        //局域网内所有在线设备
        private static List<string> LANIPs = new();
        //获取本机ip地址
        public static async Task<string> GetPublicIPAsync()
        {
            using (var client = new WebClient())
            {
                try
                {
                    string publicIp = await client.DownloadStringTaskAsync("https://api.ipify.org");
                    return publicIp.Trim();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"获取外网IP时出错: {ex.Message}");
                    return null;
                }
            }
        }
        //获取网关和子网掩码
        public static void GetLocalNetworkInfo(out string gateway, out string subnetMask)
        {
            gateway = null;
            subnetMask = null;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProps = ni.GetIPProperties();

                    foreach (GatewayIPAddressInformation g in ipProps.GatewayAddresses)
                    {
                        gateway = g.Address.ToString();
                        break;
                    }

                    foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            subnetMask = ip.IPv4Mask.ToString();
                            break;
                        }
                    }

                    if (gateway != null && subnetMask != null)
                    {
                        break;
                    }
                }
            }
        }
        //扫描局域网内所有在线设备，获取所有在线设备
        public static List<string> ScanLocalNetwork(string subnet, int timeout = 1000)
        {
            var pingTasks = new List<Task>();

            for (int i = 1; i < 255; i++)
            {
                string ip = $"{subnet}.{i}";
                pingTasks.Add(Task.Run(() => PingDevice(ip, timeout)));
            }

            Task.WaitAll(pingTasks.ToArray());
            return LANIPs;
            
        }
        private static async Task PingDevice(string ipAddress, int timeout)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = await ping.SendPingAsync(ipAddress, timeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        LANIPs.Add(ipAddress);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ping {ipAddress} 时出错: {ex.Message}");
                }
            }
        }

        // 获取子网基地址
        public static string GetSubnet()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            byte[] ipBytes = ip.Address.GetAddressBytes();
                            byte[] maskBytes = ip.IPv4Mask.GetAddressBytes();

                            byte[] subnetBytes = new byte[ipBytes.Length];
                            for (int i = 0; i < ipBytes.Length; i++)
                            {
                                subnetBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                            }

                            IPAddress subnet = new IPAddress(subnetBytes);
                            string[] subnetParts = subnet.ToString().Split('.');
                            return $"{subnetParts[0]}.{subnetParts[1]}.{subnetParts[2]}";
                        }
                    }
                }
            }

            return null;
        }
    }
}
