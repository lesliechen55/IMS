using System;
using System.Collections.Generic;
using System.Text;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ActivityCouponPageDataOutput
    {
        public long FActivityCouponId { get; set; }

        /// <summary>
        /// 优惠活动ID
        /// </summary>
        public long FActivityId { get; set; }

        /// <summary>
        /// 优惠活动名称
        /// </summary>
        public string FActivityName { get; set; }

        /// <summary>
        /// 订单详情ID
        /// </summary>
        public long? FPurchaseOrderId { get; set; }


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

        /// <summary>
        /// 优惠金额
        /// </summary>
        public Decimal FActualDiscount { get; set; }

    }
}
