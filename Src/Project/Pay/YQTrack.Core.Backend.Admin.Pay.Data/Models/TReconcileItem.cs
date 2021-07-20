using System;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TReconcileItem
    {
        public long FReconcileItemId { get; set; }
        public long FReconcileId { get; set; }
        public long FPaymentId { get; set; }
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
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public virtual TReconcile FReconcile { get; set; }
    }
}
