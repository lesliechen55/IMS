using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ActivityCouponPageDataResponse
    {
        public string ActivityCouponId { get; set; }

        /// <summary>
        /// 优惠活动ID
        /// </summary>
        public long ActivityId { get; set; }

        /// <summary>
        /// 优惠活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 订单详情ID
        /// </summary>
        public string PurchaseOrderId { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 优惠券开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 优惠券结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public CouponStatus Status { get; set; }

        /// <summary>
        /// 享受的优惠
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public Decimal ActualDiscount { get; set; }

    }
}
