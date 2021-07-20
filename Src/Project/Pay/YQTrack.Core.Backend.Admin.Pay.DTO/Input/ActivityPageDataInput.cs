using System;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class ActivityPageDataInput : PageInput
    {
        public string Keyword { get; set; }
        public int? ActivityType { get; set; }
        public int? ProductId { get; set; }
        public int? ActivityStatus { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}