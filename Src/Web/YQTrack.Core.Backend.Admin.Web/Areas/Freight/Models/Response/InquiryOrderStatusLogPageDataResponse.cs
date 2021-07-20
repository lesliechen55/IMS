using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    public class InquiryOrderStatusLogPageDataResponse
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Desc { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateBy { get; set; }
    }
}