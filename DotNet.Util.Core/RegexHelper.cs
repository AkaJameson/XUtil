using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Xin.DotnetUtil
{
    public class RegexHelper
    {
        private static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }
        /// <summary>
        /// 获取已经匹配到的集合
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static MatchCollection FindMatches(string input, string pattern)
        {
            return Regex.Matches(input, pattern);
        }
        /// <summary>
        /// 校验数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            string pattern = "^[0-9]*$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// n位数字
        /// </summary>
        /// <param name="input"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsNLengthNumber(string input, int n)
        {
            string pattern = $"^\\d{{{n}}}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 至少是n位的数字
        /// </summary>
        /// <param name="input"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsAtLeastNLengthNumber(string input, int n)
        {
            string pattern = $"^\\d{{{n},}}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// M-n位的数字
        /// </summary>
        /// <param name="input"></param>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool IsMToNLengthNumber(string input, int m, int n)
        {
            string pattern = $"^\\d{{{m},{n}}}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 0和非0开头的数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsZeroOrNonZeroStartingNumber(string input)
        {
            string pattern = "^(0|[1-9][0-9]*)$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 非0开头最多带两位小数的数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNonZeroStartingNumberWithTwoDecimals(string input)
        {
            string pattern = "^([1-9][0-9]*)+(\\.[0-9]{1,2})?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 带1-2位小数的正数或者负数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumberWithOneOrTwoDecimals(string input)
        {
            string pattern = "^(\\-)?\\d+(\\.\\d{1,2})?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 正数负数或者小数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPositiveNegativeOrDecimal(string input)
        {
            string pattern = "^(\\-|\\+)?\\d+(\\.\\d+)?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 带有两位小数的正实数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPositiveRealNumberWithTwoDecimals(string input)
        {
            string pattern = "^[0-9]+(\\.[0-9]{2})?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 带有1-3位小数的正实数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPositiveRealNumberWithOneToThreeDecimals(string input)
        {
            string pattern = "^[0-9]+(\\.[0-9]{1,3})?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 非零正整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNonZeroPositiveInteger(string input)
        {
            string pattern = "^[1-9]\\d*$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 非零的负整数：^\-[1-9][]0-9"*$ 或 ^-[1-9]\d*$
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNonZeroNegativeInteger(string input)
        {
            string pattern = "^\\-[1-9]\\d*$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 非负整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNonNegativeInteger(string input)
        {
            string pattern = "^\\d+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 非正整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNonPositiveInteger(string input)
        {
            string pattern = "^-[1-9]\\d*|0$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 非负浮点数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNonNegativeFloat(string input)
        {
            string pattern = "^\\d+(\\.\\d+)?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 非正浮点数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNonPositiveFloat(string input)
        {
            string pattern = "^((\\-\\d+(\\.\\d+)?)|0(\\.0+)?)$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 正浮点数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPositiveFloat(string input)
        {
            string pattern = "^[1-9]\\d*\\.\\d*|0\\.\\d*[1-9]\\d*$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 负浮点数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNegativeFloat(string input)
        {
            string pattern = "^-[1-9]\\d*\\.\\d*|0\\.\\d*[1-9]\\d*$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 浮点数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsFloat(string input)
        {
            string pattern = "^(-?\\d+)(\\.\\d+)?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 汉字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseCharacter(string input)
        {
            string pattern = "^[\\u4e00-\\u9fa5]{0,}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 英文或者数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlphanumeric(string input)
        {
            string pattern = "^[A-Za-z0-9]+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 长度n-m的所有字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>

        public static bool IsLengthnTom(string input,int n,int m)
        {
            string pattern = $"^.{{{n},{m}}}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 由26个英文字母组成的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlphabet(string input)
        {
            string pattern = "^[A-Za-z]+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 由26个大写英文字母组成的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUpperCaseAlphabet(string input)
        {
            string pattern = "^[A-Z]+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 由26个小写英文字母组成的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLowerCaseAlphabet(string input)
        {
            string pattern = "^[a-z]+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAlphanumericUnderscore(string input)
        {
            string pattern = "^\\w+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 中文、英文、数字包括下划线
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseEnglishNumericUnderscore(string input)
        {
            string pattern = "^[\\u4E00-\\u9FA5A-Za-z0-9_]+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 中文、英文、数字但不包括下划线等符号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseEnglishNumeric(string input)
        {
            string pattern = "^[\\u4E00-\\u9FA5A-Za-z0-9]+$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 可以输入含有^%&',;=?$\"等特殊字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsSpecialCharacters(string input)
        {
            string pattern = "[^%&',;=?$\\x22]+";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 禁止输入含有~的字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotTilde(string input)
        {
            string pattern = "[^~\\x22]+";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 判断是否是Email地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            string pattern = "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 判断域名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDomain(string input)
        {
            string pattern = "[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(/.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+/.?";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 是否是URL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUrl(string input)
        {
            string pattern = "[a-zA-z]+://[^\\s]*";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 是否是手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string input)
        {
            string pattern = "^(13[0-9]|14[5|7]|15[0-9]|18[0-9])\\d{8}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 电话号码("XXX-XXXXXXX"、"XXXX-XXXXXXXX"、"XXX-XXXXXXX"、"XXX-XXXXXXXX"、"XXXXXXX"和"XXXXXXXX)：
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsTelephoneNumber(string input)
        {
            string pattern = "^(\\(\\d{3,4}-)|\\d{3.4}-)?\\d{7,8}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 国内电话号码(0511-4405222、021-87888822)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseTelephoneNumber(string input)
        {
            string pattern = "\\d{3}-\\d{8}|\\d{4}-\\d{7}";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 身份证号(15位、18位数字)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIdCardNumber(string input)
        {
            string pattern = "^\\d{15}|\\d{18}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 短身份证号码(数字、字母x结尾)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsShortIdCardNumber(string input)
        {
            string pattern = "^([0-9]){7,18}(x|X)?$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAccount(string input)
        {
            string pattern = "^[a-zA-Z][a-zA-Z0-9_]{4,15}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 密码(以字母开头，长度在6~18之间，只能包含字母、数字和下划线)：
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPassword(string input)
        {
            string pattern = "^[a-zA-Z]\\w{5,17}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 强密码(必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-10之间)：
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsStrongPassword(string input)
        {
            string pattern = "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 日期格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDateFormat(string input)
        {
            string pattern = "^\\d{4}-\\d{1,2}-\\d{1,2}$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 一年的12个月(01～09和1～12)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMonth(string input)
        {
            string pattern = "^(0?[1-9]|1[0-2])$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 一个月的31天(01～09和1～31)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDay(string input)
        {
            string pattern = "^((0?[1-9])|((1|2)[0-9])|30|31)$";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// IP地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public static bool IsIpAddress(string input)
        {
            string pattern = "\\d+\\.\\d+\\.\\d+\\.\\d+";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// Ip-v4地址 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIPv4Address(string input)
        {
            string pattern = "\\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\b";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// Ip-v6地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIPv6Address(string input)
        {
            string pattern = "(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 子网掩码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNetworkMask(string input)
        {
            string pattern = "((?:(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d)\\.){3}(?:25[0-5]|2[0-4]\\d|[01]?\\d?\\d))";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// QQ号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidQQ(string input)
        {
            string pattern = "[1-9][0-9]{4,}";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 邮编号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public static bool IsValidChinesePostalCode(string input)
        {
            string pattern = "[1-9]\\d{5}(?!\\d)";
            return IsMatch(input, pattern);
        }
        /// <summary>
        /// 中国地区号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChinaUnicomPhoneNumber(string input)
        {
            string pattern = "^(13[0-2]|15[5-6]|18[5-6])\\d{8}$";
            return IsMatch(input, pattern);
        }
    }
}
