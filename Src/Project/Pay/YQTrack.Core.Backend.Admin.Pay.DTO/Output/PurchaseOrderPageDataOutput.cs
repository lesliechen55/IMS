using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class PurchaseOrderPageDataOutput
    {
        public long FPurchaseOrderId { get; set; }
        public UserPlatformType FUserPlatformType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public ServiceType FServiceType { get; set; }
        public string FName { get; set; }
        public decimal FSalePrice { get; set; }
        public decimal? FPaymentAmount { get; set; }
        public long FUserId { get; set; }
        public string FnickName { get; set; }//by austin 21-07-12 add
        public PurchaseOrderStatus FStatus { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public bool FIsSubscriptionConflict { get; set; }

        /// <summary>
        /// 增加返回源订单号方便查看当前订单的源头
        /// </summary>
        public long FOriginalOrderId { get; set; }
    }
}
