using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TOfflinePayment
    {
        public long FOfflinePaymentId { get; set; }
        public long FUserId { get; set; }
        public string FTransferPhotoUrl { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FAmount { get; set; }
        public decimal FCalculateAmount { get; set; }
        public string FTransferNo { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public OfflineAuditStatus FStatus { get; set; }
        public DateTime? FHandleTime { get; set; }
        public string FRejectReason { get; set; }
        public string FRemark { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long? FUpdateBy { get; set; }
        public DateTime? FUpdateAt { get; set; }
    }
}
