using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request
{
    public class ReportPageDataRequest : PageRequest
    {
        public string ChannelName { get; set; }
        public string CompanyName { get; set; }
        public ProcessReportStatusEnum? ProcessReportStatus { get; set; }
    }
}