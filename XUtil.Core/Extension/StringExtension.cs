namespace XUtil.Core.Extension
{
    public static class StringExtension
    {
        public static bool TrySpilt(string content, char param, out string[] spilts)
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
        


    }
}
