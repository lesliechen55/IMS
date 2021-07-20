using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Input
{
    public class ReportPageDataInput : PageInput
    {
        public string ChannelName { get; set; }
        public string CompanyName { get; set; }
        public ProcessReportStatusEnum? ProcessReportStatus { get; set; }
    }
}