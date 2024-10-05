using System.Security.Cryptography;
using System.Text;

namespace XUtil.Core.Securencryption
{
    /// <summary>
    /// 高级加密标准(AES,Advanced Encryption Standard)为最常见的对称加密算法
    /// </summary>
    public class AESHelper
    {
        private static readonly byte[] DefaultKey = Encoding.UTF8.GetBytes("helloworld999373"); // 16字节密钥
        private static readonly byte[] DefaultIV = Encoding.UTF8.GetBytes("373999dlrowolleh"); // 16字节IV
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">要加密的明文</param>
        /// <param name="key">加密密钥(16, 24或32字节)</param>
        /// <param name="iv">初始化向量(16字节)</param>
        /// <returns>加密后的字符串（Base64格式)</returns>
        public static string Encrypt(string plainText, byte[] key = null, byte[] iv = null)
        {
            key ??= DefaultKey;
            iv ??= DefaultIV;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="cipherText">要解密的密文(Base64格式)</param>
        /// <param name="key">解密密钥（16, 24或32字节）</param>
        /// <param name="iv">初始化向量（16字节）</param>
        /// <returns>解密后的明文</returns>
        public static string Decrypt(string cipherText, byte[] key = null, byte[] iv = null)
        {
            key ??= DefaultKey;
            iv ??= DefaultIV;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 验证给定的明文在加密后是否与提供的密文匹配
        /// </summary>
        /// <param name="plainText">要验证的明文</param>
        /// <param name="cipherText">要比较的密文(Base64格式)</param>
        /// <param name="key">加密密钥(16, 24或32字节)</param>
        /// <param name="iv">初始化向量(16字节)</param>
        /// <returns>如果明文加密后与提供的密文匹配，则返回true；否则返回false。</returns>
        public static bool VerifyEncryption(string plainText, string cipherText, byte[] key = null, byte[] iv = null)
        {
            string encryptedText = Encrypt(plainText, key, iv);
            return StringComparer.Ordinal.Compare(encryptedText, cipherText) == 0;
        }
    }
}
