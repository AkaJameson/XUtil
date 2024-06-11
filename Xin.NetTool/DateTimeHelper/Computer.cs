using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xin.DotnetUtil.DateTimeHelper
{
    public class TimeComputer
    {
        #region diff
        /// <summary>
        /// 毫秒时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double DiffMilliSecond(DateTime start, DateTime end)
        {
            return (end - start).TotalMilliseconds;
        }
        /// <summary>
        /// 秒时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double DiffSecound(DateTime start, DateTime end)
        {
            return (end - start).TotalSeconds;
        }
        /// <summary>
        /// 分钟时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double DiffMinute(DateTime start, DateTime end)
        {
            return (end - start).TotalMinutes;
        }
        /// <summary>
        /// 小时时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double diffHour(DateTime start, DateTime end)
        {
            return (end - start).TotalHours;
        }
        /// <summary>
        /// 日时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double diffDay(DateTime start, DateTime end)
        {
            return (end - start).TotalDays;
        }
        /// <summary>
        /// 周时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double diffWeek(DateTime start, DateTime end)
        {
            return (end - start).TotalDays / 7;
        }
        /// <summary>
        /// 月时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double diffMonth(DateTime start, DateTime end)
        {
            return (end.Year - start.Year) * 12 + end.Month - start.Month;
        }
        /// <summary>
        /// 年时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double diffYear(DateTime start, DateTime end)
        {
            return (end - start).Days / 365;
        }
        #endregion

        #region 当前时间
        /// <summary>
        /// 获取当前时间的"yyyy-MM-dd"
        /// </summary>
        /// <returns></returns>
        public static string GetNowStandardDayTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取当前时间的"HH:mm:ss"
        /// </summary>
        /// <returns></returns>
        public static string GetNowStandardTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        /// <summary>
        /// 返回当前时间的标准时间格式yyyy-MM-dd HH:mm:ss:fffffff
        /// </summary>
        /// <returns>yyyy-MM-dd HH:mm:ss:fffffff</returns>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }
		/// <summary>
        /// 获取当前时间戳（可以当作唯一标识)
        /// </summary>
        /// <returns></returns>
		public static long GetUtcTimeTicks()
		{
            return DateTime.UtcNow.Ticks();
		}
        #endregion

        #region convert
        /// <summary>
        /// 转换时间为unix时间戳(date为UTC时间)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origintime= new DateTime(1970,1,1,0,0,0,0);
            TimeSpan span = date - origintime;
            return Math.Floor(span.TotalSeconds);
        }

        #endregion

        #region 日常判断
        /// <summary>
        /// 判断是否是闰年
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsLeapYear(DateTime dateTime)
        {
            return DateTime.IsLeapYear(dateTime.Year);
        }
        /// <summary>
        /// 判断这今天周几
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string DayInWeek(DateTime dateTime,bool isChinese)
        {
            string[] week = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return isChinese ? week[(int)dateTime.DayOfWeek]:dateTime.DayOfWeek.ToString();
        }
        /// <summary>
        /// 根据年和天数获取日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime GetDateWithYearAndDay(int year, int day)
        {
            return new DateTime(year, 1, 1).AddDays(day - 1);
        }
        /// <summary>
        /// 获取一年中的第几周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double GetWeekofYear(DateTime dateTime)
        {
            return Math.Ceiling((double)dateTime.DayOfYear / 7);
        }
        /// <summary>
        /// 获取一年中的第几天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static double GetDayofYear(DateTime dateTime)
        {
            return dateTime.DayOfYear;
        }
        #endregion

    }
}
