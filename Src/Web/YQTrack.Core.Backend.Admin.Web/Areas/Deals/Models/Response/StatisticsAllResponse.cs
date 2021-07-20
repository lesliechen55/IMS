using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Deals.Models.Response
{
    public class StatisticsAllResponse
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int ClickCount { get; set; }

        /// <summary>
        /// 点击变化率
        /// </summary>
        public decimal ClickRate { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        public int TransactionCount { get; set; }

        /// <summary>
        /// 交易变化率
        /// </summary>
        public decimal TransactionRate { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal PaymentCount { get; set; }

        /// <summary>
        /// 佣金变化率
        /// </summary>
        public decimal PaymentRate { get; set; }

        /// <summary>
        /// 转化率
        /// </summary>
        public decimal Conversion { get; set; }

        /// <summary>
        /// 转化率变化率
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// ECPC
        /// </summary>
        public decimal ECPC { get; set; }

        /// <summary>
        /// ECPC变化率
        /// </summary>
        public decimal ECPCRate { get; set; }

        /// <summary>
        /// 统计时间(按周统计 2019-W10)
        /// </summary>
        public string StatisticsDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; } = DateTime.UtcNow;
    }
}
