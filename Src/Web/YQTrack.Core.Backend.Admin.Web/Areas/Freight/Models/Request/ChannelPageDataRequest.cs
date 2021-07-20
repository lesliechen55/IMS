using System;
using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request
{
    public class ChannelPageDataRequest : PageRequest
    {
        [DisplayName("渠道ID")]
        public long? Id { get; set; }

        [DisplayName("渠道名称")]
        public string Name { get; set; }

        [DisplayName("发布开始日期")]
        public DateTime? PublishStartTime { get; set; }

        [DisplayName("发布结束日期")]
        public DateTime? PublishEndTime { get; set; }

        [DisplayName("过期开始日期")]
        public DateTime? ExpireStartTime { get; set; }

        [DisplayName("过期结束日期")]
        public DateTime? ExpireEndTime { get; set; }

        [DisplayName("渠道状态")]
        public ChannelState? Status { get; set; }
    }
}