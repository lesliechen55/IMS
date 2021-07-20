using System;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Output
{
    /// <summary>
    /// 语言模板列表
    /// </summary>
    public class TemplatePageDataOutput
    {
        /// <summary>
        /// 模版Id
        /// </summary>
        public long FTemplateId { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string FLanguage { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string FProjectName { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public ChannelSend FChannel { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string FTemplateName { get; set; }

        /// <summary>
        /// 模板标题
        /// </summary>
        public string FTemplateTitle { get; set; }
        //public string FtemplateBody { get; set; }
    }
}
