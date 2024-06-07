using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.Securencryption
{
    /// <summary>
    /// MD5加密是最常见的加密方式，因为MD5是不可逆的，所以很多系统的密码都是用MD5加密保存的。
    ///MD5加密不可逆，但可以对明文再次加密，进行两次加密的密文进行对比
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// Md5一次加严
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ComputeMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return ConvertBytesToHexString(hashBytes);
            }
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ConvertBytesToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// MD5二次加严
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ComputeDoubleMd5Hash(string input)
        {
            string firstHash = ComputeMd5Hash(input);
            return ComputeMd5Hash(firstHash);
        }

        /// <summary>
        /// 二次加严验证
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="hash">加密后的hash值</param>
        /// <returns></returns>
        public static bool VerifyDoubleMd5Hash(string input, string hash)
        {
            string computedHash = ComputeDoubleMd5Hash(input);
            return StringComparer.OrdinalIgnoreCase.Compare(computedHash, hash) == 0;
        }
        /// <summary>
        /// 一次加严验证
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="hash">加密后的hash值</param>
        /// <returns></returns>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string computedHash = ComputeMd5Hash(input);
            return StringComparer.OrdinalIgnoreCase.Compare(computedHash, hash) == 0;
        }
    }   
}
