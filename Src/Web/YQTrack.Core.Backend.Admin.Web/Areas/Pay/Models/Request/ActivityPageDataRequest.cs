using System;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ActivityPageDataRequest : PageRequest
    {
        public string Keyword { get; set; }

        public int? ActivityType { get; set; }

        public int? ProductId { get; set; }

        public int? ActivityStatus { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}