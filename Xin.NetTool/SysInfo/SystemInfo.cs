using System.Management;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
namespace Xin.NetTool.SysInfo
{
    public class SystemInfo
    {
        //获取CPU序列号
        static string GetCpuID()
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
        static string GetMacAddress()
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
        static string GetHDid()
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
        //获取Ip地址
        static string GetIPAddress()
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
        static string GetOsType()
        {
            try
            {
                return System.Environment.OSVersion.ToString();
            }
        }
        //获取主机名
        static string GetComputerName()
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
        static string GetOSArchitecture()
        {
            try
            {
                return RuntimeInformation.OSArchitecture.ToString();.
            }
            catch
            {
                return "unknow";
            }
            finally { }
        }
        //获取操作系统名称
        static string GetOSDescription()
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
    }

}
