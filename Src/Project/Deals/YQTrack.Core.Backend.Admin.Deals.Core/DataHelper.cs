using System;

namespace YQTrack.Core.Backend.Admin.Deals.Core
{
    public static class DataHelper
    {
        /// <summary>
        /// 获取增长率
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static decimal GetGrowthRate(decimal d1, decimal d2)
        {
            if (d1 == 0)
            {
                return 0;
            }
            if (d2 == 0)
            {
                return 100;
            }

            return decimal.Round((d1 - d2) / d2 * 100, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 获取比率
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static decimal GetRate(decimal d1, decimal d2)
        {
            if (d1 == 0)
            {
                return 0;
            }
            if (d2 == 0)
            {
                return 100;
            }
            return d1 / d2;

        }
    }
}
