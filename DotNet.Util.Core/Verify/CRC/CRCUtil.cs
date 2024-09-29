namespace Xin.DotnetUtil.Verify.CRC
{
    /// <summary>
    /// CRC校验工具
    /// </summary>
    public class CRCUtil
    {
        private static Dictionary<CRCCrcAlgorithm, CRCParamter> standradCrcParamterDict = new Dictionary<CRCCrcAlgorithm, CRCParamter>
{
    { CRCCrcAlgorithm.CRC_8, new CRCParamter { BitWidth = 8, Polynomial = 0x07, InitValue = 0x00, XORValue = 0x00, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_8_CDMA2000, new CRCParamter { BitWidth = 8, Polynomial = 0x9B, InitValue = 0xFF, XORValue = 0x00, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_8_DARC, new CRCParamter { BitWidth = 8, Polynomial = 0x39, InitValue = 0x00, XORValue = 0x00, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_8_DVB_S2, new CRCParamter { BitWidth = 8, Polynomial = 0xD5, InitValue = 0x00, XORValue = 0x00, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_8_EBU, new CRCParamter { BitWidth = 8, Polynomial = 0x1D, InitValue = 0xFF, XORValue = 0x00, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_8_I_CODE, new CRCParamter { BitWidth = 8, Polynomial = 0x1D, InitValue = 0xFD, XORValue = 0x00, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_8_ITU, new CRCParamter { BitWidth = 8, Polynomial = 0x07, InitValue = 0x00, XORValue = 0x55, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_8_MAXIM, new CRCParamter { BitWidth = 8, Polynomial = 0x31, InitValue = 0x00, XORValue = 0x00, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_8_ROHC, new CRCParamter { BitWidth = 8, Polynomial = 0x07, InitValue = 0xFF, XORValue = 0x00, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_8_WCDMA, new CRCParamter { BitWidth = 8, Polynomial = 0x9B, InitValue = 0x00, XORValue = 0x00, InputReverse = true, OutPutReverse = true } },

    { CRCCrcAlgorithm.CRC_16_ARC, new CRCParamter { BitWidth = 16, Polynomial = 0x8005, InitValue = 0x0000, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_AUG_CCITT, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0x1D0F, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_BUYPASS, new CRCParamter { BitWidth = 16, Polynomial = 0x8005, InitValue = 0x0000, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_CCITT_FALSE, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0xFFFF, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_CDMA2000, new CRCParamter { BitWidth = 16, Polynomial = 0xC867, InitValue = 0xFFFF, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_CCITT, new CRCParamter { BitWidth = 16, Polynomial = 0x1021,InitValue = 0x0000, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_DDS_110, new CRCParamter { BitWidth = 16, Polynomial = 0x8005, InitValue = 0x800D, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_DECT_R, new CRCParamter { BitWidth = 16, Polynomial = 0x0589, InitValue = 0x0000, XORValue = 0x0001, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_DECT_X, new CRCParamter { BitWidth = 16, Polynomial = 0x0589, InitValue = 0x0000, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_DNP, new CRCParamter { BitWidth = 16, Polynomial = 0x3D65, InitValue = 0x0000, XORValue = 0xFFFF, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_EN_13757, new CRCParamter { BitWidth = 16, Polynomial = 0x3D65, InitValue = 0x0000, XORValue = 0xFFFF, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_GENIBUS, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0xFFFF, XORValue = 0xFFFF, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_KERMIT, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0x0000, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_MAXIM, new CRCParamter { BitWidth = 16, Polynomial = 0x8005, InitValue = 0x0000, XORValue = 0xFFFF, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_MCRF4XX, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0xFFFF, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_MODBUS, new CRCParamter { BitWidth = 16, Polynomial = 0x8005, InitValue = 0xFFFF, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_RIELLO, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0xB2AA, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_T10_DIF, new CRCParamter { BitWidth = 16, Polynomial = 0x8BB7, InitValue = 0x0000, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_TELEDISK, new CRCParamter { BitWidth = 16, Polynomial = 0xA097, InitValue = 0x0000, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_16_TMS37157, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0x89EC, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_USB, new CRCParamter { BitWidth = 16, Polynomial = 0x8005, InitValue = 0xFFFF, XORValue = 0xFFFF, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_X25, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0xFFFF, XORValue = 0xFFFF, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_16_XMODEM, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0x0000, XORValue = 0x0000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_A, new CRCParamter { BitWidth = 16, Polynomial = 0x1021, InitValue = 0xC6C6, XORValue = 0x0000, InputReverse = true, OutPutReverse = true } },

    { CRCCrcAlgorithm.CRC_32, new CRCParamter { BitWidth = 32, Polynomial = 0x04C11DB7, InitValue = 0xFFFFFFFF, XORValue = 0xFFFFFFFF, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_32_BZIP2, new CRCParamter { BitWidth = 32, Polynomial = 0x04C11DB7, InitValue = 0xFFFFFFFF, XORValue = 0xFFFFFFFF, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_32_JAMCRC, new CRCParamter { BitWidth = 32, Polynomial = 0x04C11DB7, InitValue = 0xFFFFFFFF, XORValue = 0x00000000, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_32_MPEG2, new CRCParamter { BitWidth = 32, Polynomial = 0x04C11DB7, InitValue = 0xFFFFFFFF, XORValue = 0x00000000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_32_POSIX, new CRCParamter { BitWidth = 32, Polynomial = 0x04C11DB7, InitValue = 0x00000000, XORValue = 0xFFFFFFFF, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_32_SATA, new CRCParamter { BitWidth = 32, Polynomial = 0x04C11DB7, InitValue = 0x52325032, XORValue = 0x00000000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_32_XFER, new CRCParamter { BitWidth = 32, Polynomial = 0x000000AF, InitValue = 0x00000000, XORValue = 0x00000000, InputReverse = false, OutPutReverse = false } },
    { CRCCrcAlgorithm.CRC_32C, new CRCParamter { BitWidth = 32, Polynomial = 0x1EDC6F41, InitValue = 0xFFFFFFFF, XORValue = 0xFFFFFFFF, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_32D, new CRCParamter { BitWidth = 32, Polynomial = 0xA833982B, InitValue = 0xFFFFFFFF, XORValue = 0xFFFFFFFF, InputReverse = true, OutPutReverse = true } },
    { CRCCrcAlgorithm.CRC_32Q, new CRCParamter { BitWidth = 32, Polynomial = 0x814141AB, InitValue = 0x00000000, XORValue = 0x00000000, InputReverse = false, OutPutReverse = false } }
};
        

        public static CRCParamter GetCrcParamterFromDict(CRCCrcAlgorithm cRCCrcAlgorithm)
        {
            standradCrcParamterDict.TryGetValue(cRCCrcAlgorithm, out CRCParamter crcParamterValue);
            return crcParamterValue;
        }


        // 反转
        private static uint Reflect(uint value, int bitWidth)
        {
            uint reflection = 0;

            for (int i = 0; i < bitWidth; i++)
            {
                if ((value & 1) != 0)
                {
                    reflection |= (1u << (bitWidth - 1 - i));
                }
                value >>= 1;
            }

            return reflection;
        }

        // 按字节反转
        public static byte ReflectByte(byte value)
        {
            byte reflected = 0;
            for (int i = 0; i < 8; i++)
            {
                reflected <<= 1;
                if ((value & 1) == 1)
                {
                    reflected |= 1;
                }
                value >>= 1;
            }
            return reflected;
        }
        private static uint ComputeCRC(byte[] data, int bitWidth, uint polynomial, uint initialValue, uint xorOut, bool inputReflect, bool outputReflect)
        {
            uint crc = initialValue;

            foreach (byte b in data)
            {
                byte value = inputReflect ? ReflectByte(b) : b;
                crc ^= (uint)(value << (bitWidth - 8));

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & (1u << (bitWidth - 1))) != 0)
                    {
                        crc = (crc << 1) ^ polynomial;
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            if (outputReflect)
            {
                crc = Reflect(crc, bitWidth);
            }

            return (crc ^ xorOut) & ((1u << bitWidth) - 1);
        }

        // 小端转换
      
        #region 外层调用

        public static uint Compute(byte[] buffer,CRCCrcAlgorithm cRCCrcAlgorithm, int offset=0)
        {
            standradCrcParamterDict.TryGetValue(cRCCrcAlgorithm, out CRCParamter standardCRCParamter);
            if(standardCRCParamter == null)
            {
                throw new Exception("当前标准CRC算法字典中不存在这个类型");
            }
            return ComputeCRC(buffer.Skip(offset).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial,standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);


        }
        public static uint Compute(byte[] buffer, CRCCrcAlgorithm cRCCrcAlgorithm, int offset, int count)
        {
            standradCrcParamterDict.TryGetValue(cRCCrcAlgorithm, out CRCParamter standardCRCParamter);
            if (standardCRCParamter == null)
            {
                throw new Exception("当前标准CRC算法字典中不存在这个类型");
            }
            return ComputeCRC(buffer.Skip(offset).Take(count).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial, standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);


        }

        public static uint Compute(byte[] buffer, CRCParamter standardCRCParamter, int offset = 0)
        {
            return ComputeCRC(buffer.Skip(offset).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial, standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);

        }


        public static uint Compute(byte[] buffer, CRCParamter standardCRCParamter, int offset,int count)
        {
            return ComputeCRC(buffer.Skip(offset).Take(count).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial, standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);
        }

        public static byte[] ComputeBytes(byte[] buffer, CRCCrcAlgorithm cRCCrcAlgorithm, int offset = 0,bool littleEndian = false)
        {
            standradCrcParamterDict.TryGetValue(cRCCrcAlgorithm, out CRCParamter standardCRCParamter);
            if (standardCRCParamter == null)
            {
                throw new Exception("当前标准CRC算法字典中不存在这个类型");
            }
            uint CrcValue = ComputeCRC(buffer.Skip(offset).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial, standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);

            byte[] result;
            switch (standardCRCParamter.BitWidth)
            {
                case 8:
                    result = new byte[] { (byte)CrcValue };
                    break;
                case 16:
                    result = BitConverter.GetBytes((ushort)CrcValue);
                    break;
                case 32:
                    result = BitConverter.GetBytes(CrcValue);
                    break;
                default:
                    throw new Exception("不支持的CRC位宽");
            }
            if (!littleEndian)
            {
                Array.Reverse(result);
            }
            return result;
        }
        public static byte[] ComputeBytes(byte[] buffer, CRCCrcAlgorithm cRCCrcAlgorithm, int offset, int count, bool littleEndian = false)
        {
            standradCrcParamterDict.TryGetValue(cRCCrcAlgorithm, out CRCParamter standardCRCParamter);
            if (standardCRCParamter == null)
            {
                throw new Exception("当前标准CRC算法字典中不存在这个类型");
            }
            uint CrcValue = ComputeCRC(buffer.Skip(offset).Take(count).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial, standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);

            byte[] result;
            switch (standardCRCParamter.BitWidth)
            {
                case 8:
                    result = new byte[] { (byte)CrcValue };
                    break;
                case 16:
                    result = BitConverter.GetBytes((ushort)CrcValue);
                    break;
                case 32:
                    result = BitConverter.GetBytes(CrcValue);
                    break;
                default:
                    throw new Exception("不支持的CRC位宽");
            }
            if (!littleEndian)
            {
                Array.Reverse(result);
            }
            return result;

        }
        public static byte[] ComputeBytes(byte[] buffer, CRCParamter standardCRCParamter, int offset = 0,bool littleEndian=false)
        {
            uint CrcValue = ComputeCRC(buffer.Skip(offset).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial, 
                standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);

            byte[] result;
            switch (standardCRCParamter.BitWidth)
            {
                case 8:
                    result = new byte[] { (byte)CrcValue };
                    break;
                case 16:
                    result = BitConverter.GetBytes((ushort)CrcValue);
                    break;
                case 32:
                    result = BitConverter.GetBytes(CrcValue);
                    break;
                default:
                    throw new Exception("不支持的CRC位宽");
            }
            if ( !littleEndian)
            {
                Array.Reverse(result);
            }
            return result;
        }
        public static byte[] ComputeBytes(byte[] buffer, CRCParamter standardCRCParamter, int offset,int count,bool littleEndian = false)
        {
            uint CrcValue = ComputeCRC(buffer.Skip(offset).Take(count).ToArray(), standardCRCParamter.BitWidth, standardCRCParamter.Polynomial, 
                standardCRCParamter.InitValue, standardCRCParamter.XORValue, standardCRCParamter.InputReverse, standardCRCParamter.OutPutReverse);

            byte[] result;
            switch (standardCRCParamter.BitWidth)
            {
                case 8:
                    result = new byte[] { (byte)CrcValue };
                    break;
                case 16:
                    result = BitConverter.GetBytes((ushort)CrcValue);
                    break;
                case 32:
                    result = BitConverter.GetBytes(CrcValue);
                    break;
                default:
                    throw new Exception("不支持的CRC位宽");
            }
            if (!littleEndian)
            {
                Array.Reverse(result);
            }
            return result;
        }

        #endregion
    }
}
