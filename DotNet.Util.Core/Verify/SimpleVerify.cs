using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.Verify
{
    public class SimpleVerify
    {
        /// <summary>
        /// 和校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte CalculateXorChecksum(byte[] data)
        {
            byte xor = 0;
            foreach (byte b in data)
            {
                xor ^= b;
            }
            return xor;
        }
        /// <summary>
        /// 异或校验
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte CalculateChecksum(byte[] data)
        {
            int sum = 0;
            foreach (byte b in data)
            {
                sum += b;
            }
            return (byte)(sum & 0xFF); // 取低位字节
        }
    }

}
