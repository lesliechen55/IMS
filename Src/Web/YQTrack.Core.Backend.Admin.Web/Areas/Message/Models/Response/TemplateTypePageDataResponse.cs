using System;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response
{
    /// <summary>
    /// 基础模板列表
    /// </summary>
    public class TemplateTypePageDataResponse
    {
        /// <summary>
        /// 模版类型Id
        /// </summary>
        public string TemplateTypeId { get; set; }

        /// <summary>
        /// 模版编号
        /// </summary>
        public string TemplateNo { get; set; }

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
        /// 描述
        /// </summary>
        public string TemplateDescribe { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public string Enable { get; set; }

        /// <summary>
        /// 是否需要渲染
        /// 0 否 1  是
        /// </summary>
        public int? IsRendering { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 数据内容Json
        /// </summary>
        public string DataJson { get; set; }

        /// <summary>
        /// 模板枚举Code
        /// </summary>
        public int TemplateCode { get; set; }
    }
}
