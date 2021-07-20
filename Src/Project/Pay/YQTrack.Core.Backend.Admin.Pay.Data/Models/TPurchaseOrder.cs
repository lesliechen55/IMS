using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TPurchaseOrder
    {
        public long FPurchaseOrderId { get; set; }
        public UserPlatformType FUserPlatformType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public ServiceType FServiceType { get; set; }
        public string FName { get; set; }
        public decimal FSalePrice { get; set; }
        public decimal? FPaymentAmount { get; set; }
        public long FUserId { get; set; }
        public PurchaseOrderStatus FStatus { get; set; }
        public DateTime FConfirmTime { get; set; }
        public DateTime? FRefundConfirmTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public long FOriginalOrderId { get; set; }
        public string FEmail { get; set; }

        public virtual ICollection<TPurchaseOrderItem> TPurchaseOrderItem { get; set; }

        /// <summary>
        /// 表明当前的Order是否是意外购买的订阅冲突导致的,默认值 否:0
        /// </summary>
        public bool FIsSubscriptionConflict { get; set; }
    }
}
