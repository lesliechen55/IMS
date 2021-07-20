using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Deals.Models.Response
{
    public class StatisticsMerResponses
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 广告商6位ID
        /// </summary>
        public int YQMerchantLibraryId { get; set; }

        /// <summary>
        /// 广告商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int ClickCount { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        public int TransactionCount { get; set; }

        /// <summary>
        /// 转化率
        /// </summary>
        public decimal Conversion { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal PaymentCount { get; set; }

        /// <summary>
        /// ECPC
        /// </summary>
        public decimal ECPC { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Prioritys { get; set; }

        /// <summary>
        /// 统计时间(按周统计 2019-W10)
        /// </summary>
        public string StatisticsDate { get; set; }

        /// <summary>
        /// 周数(从1970-01-01开始)
        /// </summary>
        public int WeekNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }
    }
}
