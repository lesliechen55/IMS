using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ActivityCouponPageDataRequest : PageRequest
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityId { get; set; }

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
        public string UserId { get; set; }

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
