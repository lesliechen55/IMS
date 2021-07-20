using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TProductSkuPrice
    {
        public long FProductSkuPriceId { get; set; }
        public long FProductSkuId { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public string FDescription { get; set; }
        public UserPlatformType FPlatformType { get; set; }
        public decimal FSaleUnitPrice { get; set; }
        public decimal FAmount { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public virtual TProductSku FProductSku { get; set; }
    }
}
