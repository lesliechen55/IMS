using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class PurchaseOrderPageDataRequest : PageRequest
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 注册邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 订单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public UserPlatformType[] PlatformType { get; set; }

        /// <summary>
        /// 货币类型
        /// </summary>
        public CurrencyType? CurrencyType { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public ServiceType[] ServiceType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentProvider[] ProviderId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public PurchaseOrderStatus[] PurchaseOrderStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 冲突订单
        /// </summary>
        public bool? ConflictOrder { get; set; }
    }
}