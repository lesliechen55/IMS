using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    public class ReportPageDataResponse
    {
        public string Id { get; set; }
        public string ChannelTitle { get; set; }
        public string CompanyName { get; set; }
        public string ReasonType { get; set; }
        public string Detail { get; set; }
        public string ReportEmail { get; set; }
        public DateTime ReportTime { get; set; }
        public string ProcessStatus { get; set; }
        public DateTime? ProcessTime { get; set; }
        public string ProcessRemark { get; set; }
        public string ProcessDescription { get; set; }
    }
}