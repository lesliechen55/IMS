using System;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Input
{
    /// <summary>
    /// 发送任务返回
    /// </summary>
    public class SendTaskEditOutput
    {
        
        /// <summary>
        /// Id
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public ObjType ObjType { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public ChannelSend Channel { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public long TemplateTypeId { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 对象详情(一对一)
        /// </summary>
        public string ObjDetails { get; set; }
    }
    
}
