using System;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request
{
    public class InquiryPageDataRequest : PageRequest
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string InquiryNo { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishStartTime { get; set; }
        public DateTime? PublishEndTime { get; set; }
        public DateTime? ExpireStartTime { get; set; }
        public DateTime? ExpireEndTime { get; set; }
        public InquiryOrderStatus? Status { get; set; }
    }
}