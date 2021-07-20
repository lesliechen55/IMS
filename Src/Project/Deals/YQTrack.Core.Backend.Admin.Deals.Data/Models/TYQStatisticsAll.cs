using System;

namespace YQTrack.Core.Backend.Admin.Deals.Data.Models
{
    public partial class TYQStatisticsAll
    {
        public long FId { get; set; }
        public int FClickCount { get; set; }
        public decimal FClickRate { get; set; }
        public int FTransactionCount { get; set; }
        public decimal FTransactionRate { get; set; }
        public decimal FPaymentCount { get; set; }
        public decimal FPaymentRate { get; set; }
        public decimal FConversion { get; set; }
        public decimal FConversionRate { get; set; }
        public decimal FECPC { get; set; }
        public decimal FECPCRate { get; set; }
        public string FStatisticsDate { get; set; }
        public int FWeekNum { get; set; }
        public DateTime FCreateDate { get; set; }
        public DateTime FModifyDate { get; set; }
    }
}
