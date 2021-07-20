using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class PaymentPageDataResponse
    {
        public string PaymentId { get; set; }
        public string ProviderId { get; set; }
        public string ServiceType { get; set; }
        public string CurrencyType { get; set; }
        public string PayerId { get; set; }
        public string PlatformType { get; set; }
        public string OrderId { get; set; }
        public string OrderName { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }

        public string ProviderTradeNo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
