using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response
{
    /// <summary>
    /// 语言模板列表
    /// </summary>
    public class TemplatePageDataResponse
    {
        /// <summary>
        /// 模版Id
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public ChannelSend Channel { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 模板标题
        /// </summary>
        public string TemplateTitle { get; set; }
    }
}
