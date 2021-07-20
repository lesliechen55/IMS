using System;
using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request
{
    public class InquiryOrderStatusLogPageDataRequest : PageRequest
    {
        [DisplayName("询价单ID")]
        public long? OrderId { get; set; }

        [DisplayName("开始日期")]
        public DateTime? StartTime { get; set; }

        [DisplayName("结束日期")]
        public DateTime? EndTime { get; set; }
    }
}