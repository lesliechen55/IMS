using System;

namespace YQTrack.Core.Backend.Admin.Deals.Data.Models
{
    public partial class TYQStatisticsMer
    {
        public long FId { get; set; }
        public int FYQMerchantLibraryId { get; set; }
        public int FClickCount { get; set; }
        public int FTransactionCount { get; set; }
        public decimal FConversion { get; set; }
        public decimal FPaymentCount { get; set; }
        public decimal FECPC { get; set; }
        public byte FPrioritys { get; set; }
        public string FStatisticsDate { get; set; }
        public int FWeekNum { get; set; }
        public DateTime FCreateDate { get; set; }
        public DateTime FModifyDate { get; set; }
    }
}
