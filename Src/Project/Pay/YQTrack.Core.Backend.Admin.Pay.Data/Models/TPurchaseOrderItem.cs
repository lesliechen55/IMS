using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TPurchaseOrderItem
    {
        public long FPurchaseOrderItemId { get; set; }
        public long FPurchaseOrderId { get; set; }
        public string FProductSkuCode { get; set; }
        public string FProductSkuMappingCode { get; set; }
        public string FProductSkuName { get; set; }
        public UserMemberLevel FMemberLevel { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FSaleUnitPrice { get; set; }
        public decimal FUnitPrice { get; set; }
        public int FQuantity { get; set; }
        public decimal FPaymentAmount { get; set; }
        public string FBusiness { get; set; }
        public DateTime? FStartTime { get; set; }
        public DateTime? FStopTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public virtual TPurchaseOrder FPurchaseOrder { get; set; }
    }
}
