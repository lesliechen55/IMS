using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TPayment
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
        public string FProviderTradeUrl { get; set; }
        public string FProviderTradeToken { get; set; }
        public string FProviderTradeNo { get; set; }
        public string FProviderTradeStatus { get; set; }
        public string FProviderTradeDetail { get; set; }
        public DateTime? FProviderTradeTime { get; set; }
        public string FProviderRefundUrl { get; set; }
        public DateTime? FProviderRefundTime { get; set; }
        public ReconcileState FReconcileState { get; set; }
        public bool FApplyInvoice { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
