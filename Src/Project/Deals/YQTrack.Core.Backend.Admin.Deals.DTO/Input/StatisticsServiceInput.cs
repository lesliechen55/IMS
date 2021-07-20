using System;

namespace YQTrack.Core.Backend.Admin.Deals.DTO.Input
{
    public class StatisticsServiceInput
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
