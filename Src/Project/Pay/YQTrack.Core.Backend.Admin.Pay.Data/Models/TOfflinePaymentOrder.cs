using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TOfflinePaymentOrder
    {
        public long FId { get; set; }
        public long FOfflinePaymentId { get; set; }
        public long? FPurchaseOrderId { get; set; }
        public long FProductSkuId { get; set; }
        public string FProductSkuName { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FUnitPrice { get; set; }
        public int FQuantity { get; set; }
        public DateTime? FEffectiveTime { get; set; }
    }
}
