using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Deals.Models.Request
{
    /// <summary>
    /// 获取统计数据
    /// </summary>
    public class StatisticsServiceRequest
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
