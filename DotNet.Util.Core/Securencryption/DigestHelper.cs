using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XUtil.Core.Securencryption
{
    /// <summary>
    /// 摘要加密工具类，包括MD5、SHA256和SHA512加密及校验方法。
    /// </summary>
    public class DigestHelper
    {
        #region MD5

        /// <summary>
        /// 生成字符串的MD5哈希值
        /// </summary>
        public static string ComputeMd5Hash(string input, string format = "x2")
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return ConvertBytesToHexString(hashBytes, format);
            }
        }

        /// <summary>
        /// 生成字符串的MD5哈希值并返回字节数组
        /// </summary>
        public static byte[] ComputeMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return md5.ComputeHash(inputBytes);
            }
        }

        /// <summary>
        /// 验证输入字符串的MD5哈希值是否与提供的字节数组相匹配
        /// </summary>
        public static bool VerifyMD5(string input, byte[] hashCode)
        {
            byte[] inputHash = ComputeMd5Hash(input);
            return hashCode.SequenceEqual(inputHash);
        }

        #endregion

        #region SHA256

        /// <summary>
        /// 生成字符串的SHA256哈希值
        /// </summary>
        public static string ComputeSHA256(string input, string format = "x2")
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return ConvertBytesToHexString(hashBytes, format);
            }
        }

        /// <summary>
        /// 生成字符串的SHA256哈希值并返回字节数组
        /// </summary>
        public static byte[] ComputeSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return sha256.ComputeHash(inputBytes);
            }
        }

        /// <summary>
        /// 验证输入字符串的SHA256哈希值是否与提供的字节数组相匹配
        /// </summary>
        public static bool VerifySHA256(string input, byte[] hashCode)
        {
            byte[] inputHash = ComputeSHA256(input);
            return hashCode.SequenceEqual(inputHash);
        }

        #endregion

        #region SHA512

        /// <summary>
        /// 生成字符串的SHA512哈希值
        /// </summary>
        public static string ComputeSHA512(string input, string format = "x2")
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                return ConvertBytesToHexString(hashBytes, format);
            }
        }

        /// <summary>
        /// 生成字符串的SHA512哈希值并返回字节数组
        /// </summary>
        public static byte[] ComputeSHA512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return sha512.ComputeHash(inputBytes);
            }
        }

        /// <summary>
        /// 验证输入字符串的SHA512哈希值是否与提供的字节数组相匹配
        /// </summary>
        public static bool VerifySHA512(string input, byte[] hashCode)
        {
            byte[] inputHash = ComputeSHA512(input);
            return hashCode.SequenceEqual(inputHash);
        }
        #endregion

        #region 通用方法

        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        private static string ConvertBytesToHexString(byte[] bytes, string format = "x2")
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString(format));
            }
            return sb.ToString();
        }

        #endregion
    }
}
