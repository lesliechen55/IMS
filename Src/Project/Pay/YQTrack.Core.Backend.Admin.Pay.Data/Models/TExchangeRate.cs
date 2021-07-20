using System;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TExchangeRate
    {
        public long FExchangeRateId { get; set; }
        public long FFromCurrencyId { get; set; }
        public long FToCurrencyId { get; set; }
        public decimal FRate { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public virtual TCurrency FFromCurrency { get; set; }
        public virtual TCurrency FToCurrency { get; set; }
    }
}
