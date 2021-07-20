using System.ComponentModel.DataAnnotations;

namespace YQTrack.Core.Backend.Enums.Pay
{
    public enum PaymentAllStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Display(Name = "未知")]
        Unknown = 0,

        /// <summary>
        /// 支付已开始但未完成
        /// </summary>
        [Display(Name = "待支付")]
        Pending = 101,

        /// <summary>
        /// 支付成功
        /// </summary>
        [Display(Name = "支付成功")]
        Success = 200,

        /// <summary>
        /// 支付失败
        /// </summary>
        [Display(Name = "支付失败")]
        Failed = 400,

        /// <summary>
        /// 用户取消支付
        /// </summary>
        [Display(Name = "取消")]
        Cancelled = 402,

        /// <summary>
        /// 退款
        /// </summary>
        [Display(Name = "退款中")]
        Refunding = 500,

        /// <summary>
        /// 已退款成功
        /// </summary>
        [Display(Name = "已退款成功")]
        Refunded = 501,

        /// <summary>
        /// 退款失败
        /// </summary>
        [Display(Name = "退款失败")]
        RefundFailure = 502
    }
}
