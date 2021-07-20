using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TCurrency
    {
        public TCurrency()
        {
            TExchangeRateFFromCurrency = new HashSet<TExchangeRate>();
            TExchangeRateFToCurrency = new HashSet<TExchangeRate>();
        }

        public long FCurrencyId { get; set; }
        public string FName { get; set; }
        public string FDescription { get; set; }
        public string FCode { get; set; }
        public string FSign { get; set; }
        public bool FActive { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public virtual ICollection<TExchangeRate> TExchangeRateFFromCurrency { get; set; }
        public virtual ICollection<TExchangeRate> TExchangeRateFToCurrency { get; set; }
    }
}
