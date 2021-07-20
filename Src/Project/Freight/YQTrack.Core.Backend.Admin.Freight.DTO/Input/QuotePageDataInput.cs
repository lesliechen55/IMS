using System;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Input
{
    public class QuotePageDataInput : PageInput
    {
        public long? QuoteId { get; set; }
        public string QuoteNo { get; set; }
        public string InquiryNo { get; set; }
        public string CompanyName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public InquiryOrderStatus? InquiryStatus { get; set; }
        public bool? CancelStatus { get; set; }
    }
}