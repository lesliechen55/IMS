using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class PaymentPageDataOutput
    {
        public long FPaymentId { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public ServiceType FServiceType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public long? FPayerId { get; set; }
        public UserPlatformType FPlatformType { get; set; }
        public long? FOrderId { get; set; }
        public string FOrderName { get; set; }
        public decimal? FPaymentAmount { get; set; }
        public PaymentStatus FPaymentStatus { get; set; }
        
        public string FProviderTradeNo { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
