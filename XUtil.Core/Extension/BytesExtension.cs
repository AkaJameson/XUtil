using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUtil.Core.Extension
{
    public static class BytesExtension
    {
        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
        }
        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static bool IsEqualTo(this byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null)
                return false;

            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte[] SubArray(this byte[] array, int startIndex, int length)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (startIndex < 0 || length < 0 || startIndex + length > array.Length)
                throw new ArgumentOutOfRangeException();

            byte[] subArray = new byte[length];
            Array.Copy(array, startIndex, subArray, 0, length);
            return subArray;
        }

        /// <summary>
        /// 将字节数组转换为 Base64 字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 从 Base64 字符串转换为字节数组
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static byte[] FromBase64String(this string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
