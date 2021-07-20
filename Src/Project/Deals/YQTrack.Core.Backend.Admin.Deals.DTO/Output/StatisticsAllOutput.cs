using System;

namespace YQTrack.Core.Backend.Admin.Deals.DTO.Output
{
    public class StatisticsAllOutput
    {
        /// <summary>
        /// ID
        /// </summary>
        public long FId { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int FClickCount { get; set; }

        /// <summary>
        /// 点击变化率
        /// </summary>
        public decimal FClickRate { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        public int FTransactionCount { get; set; }

        /// <summary>
        /// 交易变化率
        /// </summary>
        public decimal FTransactionRate { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal FPaymentCount { get; set; }

        /// <summary>
        /// 佣金变化率
        /// </summary>
        public decimal FPaymentRate { get; set; }

        /// <summary>
        /// 转化率
        /// </summary>
        public decimal FConversion { get; set; }

        /// <summary>
        /// 转化率变化率
        /// </summary>
        public decimal FConversionRate { get; set; }

        /// <summary>
        /// ECPC
        /// </summary>
        public decimal FECPC { get; set; }

        /// <summary>
        /// ECPC变化率
        /// </summary>
        public decimal FECPCRate { get; set; }

        /// <summary>
        /// 统计时间(按周统计 2019-W10)
        /// </summary>
        public string FStatisticsDate { get; set; }

        /// <summary>
        /// 周数(从1970-01-01开始)
        /// </summary>
        public int FWeekNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime FCreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime FModifyDate { get; set; }
    }
}
