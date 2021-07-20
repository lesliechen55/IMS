using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Input
{
    /// <summary>
    /// 基础模板列表搜索条件
    /// </summary>
    public class TemplateTypePageDataInput : PageInput
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
