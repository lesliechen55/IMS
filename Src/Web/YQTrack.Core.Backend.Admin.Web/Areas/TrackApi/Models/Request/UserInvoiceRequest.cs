using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request
{
    public class UserInvoiceRequest
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }
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
