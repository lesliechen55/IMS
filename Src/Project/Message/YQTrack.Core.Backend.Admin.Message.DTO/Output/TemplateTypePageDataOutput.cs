using System;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Output
{
    /// <summary>
    /// 基础模板列表
    /// </summary>
    public class TemplateTypePageDataOutput
    {
        /// <summary>
        /// 模版类型Id
        /// </summary>
        public long FTemplateTypeId { get; set; }

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
        /// 描述
        /// </summary>
        public string FTemplateDescribe { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public int FEnable { get; set; }

        /// <summary>
        /// 是否需要渲染
        /// 0 否 1  是
        /// </summary>
        public int? FIsRendering { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? FCreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? FUpdateTime { get; set; }

        /// <summary>
        /// 数据内容Json
        /// </summary>
        public string FDataJson { get; set; }

        /// <summary>
        /// 模板枚举Code
        /// </summary>
        public int? FTemplateCode { get; set; }

        ///// <summary>
        ///// 模板标题
        ///// </summary>
        //public string FtemplateTitle { get; set; }

        ///// <summary>
        ///// 模板内容
        ///// </summary>
        //public string FtemplateBody { get; set; }
    }
}
