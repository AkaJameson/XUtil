using System.Security.Cryptography;
using System.Text;

namespace XUtil.Core.Securencryption
{
    /// <summary>
    /// 包含 RSA 加密、解密、签名和验证功能
    /// RSA加密采用公钥加密，私钥解密的模式或者私钥加密，公钥解密的模式。
    /// 但是RSA加密有个特点，就是他对被加密的字符串有长度限制；待加密的字节数不能超过密钥的长度值除以8再减去11
    /// </summary>
    public class RSAHelper
    {
        // 生成 RSA 密钥对
        public static void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = RSA.Create())
            {
                publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            }
        }
        /// <summary>
        /// 使用RSA公钥加密字符串
        /// </summary>
        /// <param name="plainText">要加密的明文</param>
        /// <param name="publicKey">用于加密的 RSA 公钥</param>
        /// <returns>加密后的字符串(Base64格式)</returns>
        public static string Encrypt(string plainText, string publicKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                var encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
        /// <summary>
        /// 使用RSA私钥解密字符串
        /// </summary>
        /// <param name="cipherText">要解密的密文(Base64格式)</param>
        /// <param name="privateKey">用于解密的RSA私钥</param>
        /// <returns>解密后的明文。</returns>
        public static string Decrypt(string cipherText, string privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                var decryptedBytes = rsa.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        /// <summary>
        /// 使用 RSA 私钥对字符串进行签名
        /// </summary>
        /// <param name="message">要签名的消息。</param>
        /// <param name="privateKey">用于签名的 RSA 私钥。</param>
        /// <returns>签名（Base64格式）。</returns>
        public static string SignData(string message, string privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                var messageBytes = Encoding.UTF8.GetBytes(message);
                var signatureBytes = rsa.SignData(messageBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(signatureBytes);
            }
        }

        /// <summary>
        /// 使用RSA公钥验证签名
        /// </summary>
        /// <param name="message">要验证的消息。</param>
        /// <param name="signature">要验证的签名(Base64格式)</param>
        /// <param name="publicKey">用于验证签名的 RSA 公钥</param>
        /// <returns>如果签名有效，则返回true；否则返回false</returns>
        public static bool VerifyData(string message, string signature, string publicKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                var messageBytes = Encoding.UTF8.GetBytes(message);
                var signatureBytes = Convert.FromBase64String(signature);
                return rsa.VerifyData(messageBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
