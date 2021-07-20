using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class OfflinePaymentShowOutput
    {
        public long FOfflinePaymentId { get; set; }
        public long FUserId { get; set; }
        public string FEmail { get; set; }
        public string FTransferPhotoUrl { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FAmount { get; set; }
        public decimal FCalculateAmount { get; set; }
        public string FTransferNo { get; set; }
        public OfflineAuditStatus FStatus { get; set; }
        public string FRejectReason { get; set; }
        public string FRemark { get; set; }
        public DateTime FCreateAt { get; set; }
        public List<OfflinePaymentOrderOutput> PaymentList { get; set; }
    }

    public class OfflinePaymentOrderOutput
    {
        public long? FPurchaseOrderId { get; set; }
        public string FProductSkuName { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FUnitPrice { get; set; }
        public int FQuantity { get; set; }
        public DateTime? FEffectiveTime { get; set; }
    }
}