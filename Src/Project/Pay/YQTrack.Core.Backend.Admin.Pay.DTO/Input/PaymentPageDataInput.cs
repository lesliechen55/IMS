using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class PaymentPageDataInput : PageInput
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
        /// 关联支付商
        /// </summary>
        public PaymentProvider[] PaymentProvider { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PaymentStatus[] PaymentStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
