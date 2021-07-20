using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class OfflinePaymentShowResponse
    {
        public long OfflinePaymentId { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public string TransferPhotoUrl { get; set; }
        public string CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public decimal CalculateAmount { get; set; }
        public string TransferNo { get; set; }
        public string Status { get; set; }
        public string RejectReason { get; set; }
        public string Remark { get; set; }
        public DateTime CreateAt { get; set; }
        public List<OfflinePaymentOrderResponse> PaymentList { get; set; }
    }

    public class OfflinePaymentOrderResponse
    {
        public long? PurchaseOrderId { get; set; }
        public string ProductSkuName { get; set; }
        public string CurrencyType { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string EffectiveTime { get; set; }
    }
}
