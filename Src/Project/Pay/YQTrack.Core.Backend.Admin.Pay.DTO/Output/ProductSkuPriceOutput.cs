using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ProductSkuPriceOutput
    {
        public long FProductSkuPriceId { get; set; }
        public UserPlatformType FPlatformType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FSaleUnitPrice { get; set; }
        public decimal FAmount { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime FUpdateAt { get; set; }
        public string FDescription { get; set; }
    }
}