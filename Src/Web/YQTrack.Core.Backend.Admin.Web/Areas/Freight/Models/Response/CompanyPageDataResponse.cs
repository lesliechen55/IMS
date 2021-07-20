using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    public class CompanyPageDataResponse
    {
        public string CompanyId { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
        public string CheckDescHistory { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string CheckState { get; set; }
        public string Code { get; set; }
        public int ChannelValidReportTimes { get; set; }
    }
}