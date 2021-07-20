using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class PurchaseOrderShowOutput
    {
        public long FPurchaseOrderId { get; set; }
        public UserPlatformType FUserPlatformType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public ServiceType FServiceType { get; set; }
        public string FName { get; set; }
        public decimal FSalePrice { get; set; }
        public decimal? FPaymentAmount { get; set; }
        public PurchaseOrderStatus FStatus { get; set; }
        public List<PurchaseOrderItemOutput> TPurchaseOrderItem { get; set; }

        public bool IsShowPresentPage { get; set; }
        public long FUserId { get; set; }

        public bool FIsSubscriptionConflict { get; set; }

        public long FOriginalOrderId { get; set; }

    }
    public class PurchaseOrderItemOutput
    {
        public string FProductSkuCode { get; set; }
        public string FProductSkuName { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FSaleUnitPrice { get; set; }
        public decimal FUnitPrice { get; set; }
        public int FQuantity { get; set; }
        public decimal FPaymentAmount { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime? FStartTime { get; set; }
        public DateTime? FStopTime { get; set; }
    }
}
