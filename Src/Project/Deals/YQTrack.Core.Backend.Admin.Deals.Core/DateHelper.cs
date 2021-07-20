using System;
using System.Globalization;

namespace YQTrack.Core.Backend.Admin.Deals.Core
{
    public static class DateHelper
    {
        #region < 根据时间，计算当前是第几周 返回结果：2019-W12 >

        /// <summary>
        /// 根据时间，计算当前是第几周 
        /// 周一算第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>返回结果：2019-W12</returns>
        public static string GetWeekNumString(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear < 10 ? dt.Year + "-W0" + weekOfYear : dt.Year + "-W" + weekOfYear;
        }

        /// <summary>
        /// 根据时间，计算当前是第几周(从1970-01-01开始) 
        /// 周一算第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetWeekNum(DateTime dt)
        {
            TimeSpan t1 = new TimeSpan(DateTime.Parse("1970-01-01").Ticks);
            TimeSpan t2 = new TimeSpan(dt.Ticks);
            TimeSpan ts = t1.Subtract(t2).Duration();
            //1970-01-01 为周四
            if (ts.Days <= 3)
            {
                return 1;
            }

            int weeks = ((ts.Days - 3) / 7) + 1;
            if ((ts.Days - 3) % 7 != 0)
            {
                weeks++;
            }
            return weeks;
        }
        #endregion
    }
}
