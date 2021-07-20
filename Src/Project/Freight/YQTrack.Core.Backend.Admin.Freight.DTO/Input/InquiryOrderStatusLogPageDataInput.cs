using System;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Input
{
    public class InquiryOrderStatusLogPageDataInput : PageInput
    {
        public long? OrderId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}