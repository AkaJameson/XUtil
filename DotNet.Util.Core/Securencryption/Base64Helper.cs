using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.Securencryption
{
    public class Base64Helper
    {
        /// <summary>
        /// 转换成Base64编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncodeToBase64(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes);
        }
        /// <summary>
        /// 从Base64解码成UTF8编码
        /// </summary>
        /// <param name="base64Input"></param>
        /// <returns></returns>
        public static string DecodeFromBase64(string base64Input)
        {
            byte[] decodedBytes = Convert.FromBase64String(base64Input);
            return Encoding.UTF8.GetString(decodedBytes);
        }
        /// <summary>
        /// 验证base64编码
        /// </summary>
        /// <param name="input">UTF8 编码</param>
        /// <param name="base64Encoded">base64编码</param>
        /// <returns></returns>
        public static bool VerifyBase64Encoding(string input, string base64Encoded)
        {
            string encodedInput = EncodeToBase64(input);
            return StringComparer.Ordinal.Compare(encodedInput, base64Encoded) == 0;
        }
        /// <summary>
        /// 验证base64解码
        /// </summary>
        /// <param name="base64Input">Base64编码</param>
        /// <param name="decoded">解码</param>
        /// <returns></returns>
        public static bool VerifyBase64Decoding(string base64Input, string decoded)
        {
            string decodedBase64 = DecodeFromBase64(base64Input);
            return StringComparer.Ordinal.Compare(decodedBase64, decoded) == 0;
        }

    }
}
