using System;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Input
{
    /// <summary>
    /// 任务列表搜索条件
    /// </summary>
    public class SendTaskPageDataInput : PageInput
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
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }
    }
}
