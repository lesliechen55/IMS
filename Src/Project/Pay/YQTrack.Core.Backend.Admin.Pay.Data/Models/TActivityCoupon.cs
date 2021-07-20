using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YQTrack.Backend.Payment.Model.Entities;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    /// <summary>
    /// 优惠活动
    /// </summary>
    [Table("TActivityCoupon")]
    public class TActivityCoupon : Entity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long FActivityCouponId { get; set; }

        /// <summary>
        /// 优惠活动ID
        /// </summary>
        public long FActivityId { get; set; }

        /// <summary>
        /// 订单详情ID
        /// </summary>
        public long? FPurchaseOrderId { get; set; }

        /// <summary>
        ///实际享受优惠
        /// </summary>
        public decimal? FActualDiscount { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long FUserId { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string FEmail { get; set; }

        /// <summary>
        /// 优惠券开始时间
        /// </summary>
        public DateTime FStartTime { get; set; }

        /// <summary>
        /// 优惠券结束时间
        /// </summary>
        public DateTime FEndTime { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public CouponStatus FStatus { get; set; }

        /// <summary>
        /// 享受的优惠
        /// </summary>
        public string FRule { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string FSource { get; set; }

        public virtual TActivity FActivity { get; set; }
    }
}
