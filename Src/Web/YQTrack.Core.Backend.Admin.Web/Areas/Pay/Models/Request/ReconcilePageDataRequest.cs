using System;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ReconcilePageDataRequest : PageRequest
    {
        /// <summary>
        /// 关联支付商
        /// </summary>
        public PaymentProvider[] PaymentProvider { get; set; }
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