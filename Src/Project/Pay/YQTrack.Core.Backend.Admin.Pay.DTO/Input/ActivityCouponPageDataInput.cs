using System;
using System.Collections.Generic;
using System.Text;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class ActivityCouponPageDataInput : PageInput
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public long? ActivityId { get; set; }

        /// <summary>
        /// 优惠活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public long? PurchaseOrderId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public int? Status { get; set; }
    }
}
