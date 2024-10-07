namespace XUtil.Core.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 拆分
        /// </summary>
        /// <param name="content"></param>
        /// <param name="param"></param>
        /// <param name="spilts"></param>
        /// <returns></returns>
        public static bool TrySpilt(this string content, char param, out string[] spilts)
        {
            try
            {
                spilts = content.Split(param, StringSplitOptions.RemoveEmptyEntries);

                if (spilts.Length == 1)
                {
                    spilts = new string[] { content };
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                spilts = new string[] { content };
                return false;
            }
        }

        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static TValue StringParse<TValue>(this string value) where TValue : struct
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "输入不能为空");
            }

            try
            {
                // 使用 Convert.ChangeType 将字符串转换为 TValue 类型
                return (TValue)Convert.ChangeType(value, typeof(TValue));
            }
            catch (FormatException)
            {
                throw new FormatException($"The value '{value}' is not in a correct format for type {typeof(TValue).Name}.");
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"The value '{value}' cannot be converted to type {typeof(TValue).Name}.");
            }
            catch (OverflowException)
            {
                throw new OverflowException($"The value '{value}' is out of the range for type {typeof(TValue).Name}.");
            }
        }

        public static bool IsNotNull(this string value) => !string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// 检查字符串是否包含指定子字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="substring"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string substring, StringComparison comparison)
        {
            return source?.IndexOf(substring, comparison) >= 0;
        }
        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Reverse(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        /// <summary>
        /// 将字符串首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CapitalizeFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return char.ToUpper(str[0]) + str.Substring(1);
        }
        /// <summary>
        /// 清除空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpaces(this string str)
        {
            return str?.Replace(" ", string.Empty);
        }
        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string str)
        {
            return double.TryParse(str, out _);
        }
    }
}
