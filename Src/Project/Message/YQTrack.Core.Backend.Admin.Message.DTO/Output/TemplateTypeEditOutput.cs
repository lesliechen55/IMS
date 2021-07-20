using System;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Output
{
    /// <summary>
    /// 基础模板编辑
    /// </summary>
    public class TemplateTypeEditOutput
    {
        /// <summary>
        /// 基础模板ID
        /// </summary>
        public long FTemplateTypeId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public long FProjectId { get; set; }
        /// <summary>
        /// 发送通道
        /// </summary>
        public ChannelSend? FChannel { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string FTemplateName { get; set; }
        /// <summary>
        /// 模板描述
        /// </summary>
        public string FTemplateDescribe { get; set; }
        /// <summary>
        /// 样例数据
        /// </summary>
        public string FDataJson { get; set; }
        /// <summary>
        /// 模板标题
        /// </summary>
        public string FTemplateTitle { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public string FTemplateBody { get; set; }

    }
}
