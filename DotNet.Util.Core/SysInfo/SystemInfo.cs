using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
namespace XUtil.Core.SysInfo
{
    public class SystemInfo
    {
        static PerformanceCounter cpuCounter;
        static SystemInfo()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue(); // 必须先调用一次，以初始化计数器
            Thread.Sleep(1000);
        }
        //获取CPU序列号
        public static string GetCpuID()
        {
            try
            {
                string cpuInfo = "";//cpu序列号
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }

            finally { }
        }
        //获取MAC地址
        public static string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
        //获取硬盘ID
        public static string GetHDid()
        {
            try
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
        //获取本地Ip地址
        public static string GetLocalIPAddress()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        System.Array ar;
                        ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "unknow";
            }
            finally { } 
        }
        //获取操作系统类型
        public static string GetOsType()
        {
            try
            {
                return System.Environment.OSVersion.ToString();
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
        //获取主机名
        public static string GetComputerName()
        {
            try
            {
                return System.Environment.MachineName;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
        //获取操作系统架构
        public static string GetOSArchitecture()
        {
            try
            {
                return RuntimeInformation.OSArchitecture.ToString();
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
        //获取操作系统名称
        public static string GetOSDescription()
        {
            try
            {
                return RuntimeInformation.OSDescription;
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
        //获取CPU使用率
        public static float GetCPUTotalLoad()
        {
           return cpuCounter.NextValue();
        }
        //获取物理内存总量
        public static long GetTotalMemory()
        {
            var ramCounter = new PerformanceCounter("Memory", "Available Bytes");
            return (long)new PerformanceCounter("Memory", "Total Visible Memory Size").NextValue();
        }
        //获取已用内存量
        public static long GetUsedMemory()
        {
            var ramCounter = new PerformanceCounter("Memory", "Available Bytes");
            return (long)new PerformanceCounter("Memory", "Total Visible Memory Size").NextValue() - (long)ramCounter.NextValue();
        }
        //获取可用内存量
        public static long GetAvailableMemory()
        {
            var ramCounter = new PerformanceCounter("Memory", "Available Bytes");
            return (long)ramCounter.NextValue();
        }
        
    }
}
