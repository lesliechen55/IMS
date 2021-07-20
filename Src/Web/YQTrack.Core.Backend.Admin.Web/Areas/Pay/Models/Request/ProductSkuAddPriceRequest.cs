using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductSkuAddPriceRequest
    {
        public long ProductSkuId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public string Description { get; set; }
        public UserPlatformType? PlatformType { get; set; }
        public decimal SaleUnitPrice { get; set; }
        public decimal Amount { get; set; }
    }
}