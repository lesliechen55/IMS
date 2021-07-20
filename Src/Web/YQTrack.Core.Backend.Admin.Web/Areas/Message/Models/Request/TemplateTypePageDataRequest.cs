using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request
{
    public class TemplateTypePageDataRequest : PageRequest
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public long? ProjectId { get; set; }
        /// <summary>
        /// 发送通道
        /// </summary>
        public ChannelSend? ChannelId { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

    }
}
