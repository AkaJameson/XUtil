using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUtil.Core.Extension
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 检查日期是否为今天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsToday(this DateTime dateTime)
        {
            return dateTime.Date == DateTime.Today;
        }
        /// <summary>
        /// 获取年龄
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        public static int GetAge(this DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }

        /// <summary>
        /// 判断日期是否在范围内
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool IsWithin(this DateTime dateTime, DateTime start, DateTime end)
        {
            return dateTime >= start && dateTime <= end;
        }
    }
}
