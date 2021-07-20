using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ProductSkuPriceResponse
    {
        public string ProductSkuPriceId { get; set; }
        public string PlatformType { get; set; }
        public string CurrencyType { get; set; }
        public decimal SaleUnitPrice { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Description { get; set; }
    }
}