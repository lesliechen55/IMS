using System;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ReconcileItemShowOutput
    {
        public string FOrderId { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public string FProviderNo { get; set; }
        public string FProviderStatus { get; set; }
        public string FProviderType { get; set; }
        public string FProviderCurrency { get; set; }
        public string FProviderAmount { get; set; }
        public ReconcileState FReconcileState { get; set; }
        public PaymentStatus FStatus { get; set; }
        public string FDetail { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
