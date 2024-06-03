using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Xin.NetTool.Securencryption
{
    /// <summary>
    /// DES(Data Encryption Standard)是目前最为流行的加密算法之一。DES是对称的，也就是说它使用同一个密钥来加密和解密数据。
    /// DES还是一种分组加密算法，该算法每次处理固定长度的数据段，称之为分组。
    /// DES加密算法对密钥有要求，必须是8个字符，如12345678这样的。
    /// </summary>
    public class DESHelper
    {
        // 默认的密钥和IV，可以根据需要修改
        private static readonly byte[] DefaultKey = Encoding.UTF8.GetBytes("hellowor"); 
        private static readonly byte[] DefaultIV = Encoding.UTF8.GetBytes("ldhellow"); 

        /// <summary>
        /// 加密字符串，仅允许8位密钥和8位IV
        /// </summary>
        /// <param name="plainText">要加密的明文</param>
        /// <param name="key">加密密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string plainText, byte[] key = null, byte[] iv = null)
        {
            key ??= DefaultKey;
            iv ??= DefaultIV;
            if(key.Length != 8 || iv.Length != 8)
            {
                throw new ArgumentException("Key and IV must be 8 bytes long.");
            }
            using (DES des = DES.Create())
            {
                des.Key = key;
                des.IV = iv;

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 解密字符串,仅支持8位密钥和8位IV。
        /// </summary>
        /// <param name="cipherText">要解密的密文</param>
        /// <param name="key">解密密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns>解密后的明文。</returns>
        public static string Decrypt(string cipherText, byte[] key = null, byte[] iv = null)
        {
            key ??= DefaultKey;
            iv ??= DefaultIV;
            if (key.Length != 8 || iv.Length != 8)
            {
                throw new ArgumentException("Key and IV must be 8 bytes long.");
            }
            using (DES des = DES.Create())
            {
                des.Key = key;
                des.IV = iv;

                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
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
        /// <param name="plainText">验证明文。</param>
        /// <param name="cipherText">比较密文（Base64格式）。</param>
        /// <param name="key">加密密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns>如果明文加密后与提供的密文匹配，则返回true；否则返回false。</returns>
        public static bool VerifyEncryption(string plainText, string cipherText, byte[] key = null, byte[] iv = null)
        {
            string encryptedText = Encrypt(plainText, key, iv);
            return StringComparer.Ordinal.Compare(encryptedText, cipherText) == 0;
        }
    }
}
