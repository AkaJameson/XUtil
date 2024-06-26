using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.Verify.CRC
{
    public class CRCParamter
    {
        /// <summary>
        /// 宽度
        /// </summary>
        public int BitWidth { get; set; }
        /// <summary>
        /// 多项式
        /// </summary>
        public uint Polynomial { get; set; }
        /// <summary>
        /// 初始值
        /// </summary>
        public uint InitValue { get; set; }
        /// <summary>
        /// 结果异或值
        /// </summary>
        public uint XORValue { get; set; }
        /// <summary>
        /// 输入反转
        /// </summary>
        public bool InputReverse { get; set; }
        /// <summary>
        /// 输出反转
        /// </summary>
        public bool OutPutReverse { get; set; }

        /// <summary>
        /// 创建CRCParamter
        /// </summary>
        /// <param name="bitWidth">宽度</param>
        /// <param name="polynomial">多项式</param>
        /// <param name="InitValue">初始值</param>
        /// <param name="xorValue">结果异或值</param>
        /// <param name="inputReverse">输入反转</param>
        /// <param name="outputReverse">输出反转</param>
        /// <returns></returns>
        public static CRCParamter CreateCRCParamter(int bitWidth, uint polynomial, uint InitValue, uint xorValue, bool inputReverse, bool outputReverse)
        {
            return new CRCParamter { BitWidth = bitWidth, Polynomial = polynomial, InitValue = InitValue, XORValue = xorValue, InputReverse = inputReverse, OutPutReverse = outputReverse };
        }



    }




}
