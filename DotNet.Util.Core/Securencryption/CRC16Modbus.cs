using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Securencryption
{
    /// <summary>
    /// ModBusCRC校验类，提供生成CRC校验码，验证CRC校验码的功能。
    /// </summary>
    public class CRC16Modbus
    {
        private const ushort polynomial = 0xA001; 

        private static ushort[] table = new ushort[256];

        private static ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0xFFFF;

            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }

            return crc;
        }
        /// <summary>
        /// 计算CRC校验值
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] ComputeChecksumBytes(byte[] bytes)
        {
            ushort crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }

        public static bool ValidateModbusRTUCRC(byte[] data, byte[] crcBytes)
        {
            byte[] dataCrcBytes = ComputeChecksumBytes(data);

            return dataCrcBytes[0] == crcBytes[0] && dataCrcBytes[1] == crcBytes[1];
        }
        static CRC16Modbus()
        {
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }
    }
}
