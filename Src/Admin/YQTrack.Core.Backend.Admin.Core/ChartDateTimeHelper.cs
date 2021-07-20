using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class ChartDateTimeHelper
    {
        /// <summary>
        /// 根据图标日期类型返回开始时间和结束时间（日期包含当天，周包含当周，月包含当月）
        /// </summary>
        /// <param name="chartDateType"></param>
        /// <returns></returns>
        public static (DateTime startTime, DateTime endTime) InitTime(ChartDateType chartDateType)
        {
            var offset = chartDateType.GetDefaultValue();
            var utcNow = DateTime.UtcNow;
            var startTime = utcNow.Date;
            var endTime = utcNow;
            switch (chartDateType)
            {
                case ChartDateType.Day:
                    startTime = startTime.AddDays(1 - offset);
                    break;
                case ChartDateType.Week:
                    int dayOfWeek = Convert.ToInt16(endTime.DayOfWeek);
                    DateTime firstDay = endTime.AddDays(-dayOfWeek);
                    startTime = firstDay.AddDays(-7 * (offset - 1));
                    break;
                case ChartDateType.Month:
                    startTime = startTime.AddMonths(1 - offset).AddDays(1 - startTime.Day);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chartDateType), chartDateType, null);
            }
            return (startTime, endTime);
        }

        /// <summary>
        /// 根据开始和结束时间计算周的字符串集合
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetWeekFormatList(DateTime startTime, DateTime endTime)
        {
            var weekList = new List<string>();
            var startWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(startTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            var endWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(endTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            if (endTime.Year - startTime.Year > 0)
            {
                var startYearLastWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(new DateTime(startTime.Year, 12, 31), CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                var weeksFront = Enumerable.Range(startWeek, startYearLastWeek - startWeek + 1).Select(x => $"{startTime.Year}第{x}周");
                weekList.AddRange(weeksFront);
                var weeksBack = Enumerable.Range(1, endWeek).Select(x => $"{endTime.Year}第{x}周");
                weekList.AddRange(weeksBack);
            }
            else
            {
                var weeks = Enumerable.Range(startWeek, endWeek - startWeek + 1).Select(x => $"{startTime.Year}第{x}周");
                weekList.AddRange(weeks);
            }
            return weekList;
        }
    }
}