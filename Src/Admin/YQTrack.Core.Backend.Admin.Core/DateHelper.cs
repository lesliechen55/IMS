using System;

namespace YQTrack.Core.Backend.Admin.Core
{
    /// <summary>
    /// 日期时间帮助类
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// 根据字符'-'分割得到时间段
        /// </summary>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public static (DateTime? startDateTime, DateTime? enDateTime) Split(string dateRange)
        {
            if (dateRange.IsNullOrWhiteSpace()) return (null, null);
            var strings = dateRange.Split('~', StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length != 2) return (null, null);
            if (!DateTime.TryParse(strings[0], out var startDateTime) ||
                !DateTime.TryParse(strings[1], out var endDateTime))
            {
                return (null, null);
            }
            return (startDateTime, endDateTime);
        }
    }
}