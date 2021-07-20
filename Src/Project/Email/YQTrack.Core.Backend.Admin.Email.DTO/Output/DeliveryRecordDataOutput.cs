using System;
using YQTrack.Backend.Email.Enums;

namespace YQTrack.Core.Backend.Admin.Email.DTO.Output
{
    /// <summary>
    /// 邮件投递详情
    /// </summary>
    public class DeliveryRecordDataOutput
    {
        /// <summary>
        /// 邮件服务提供商类型
        /// </summary>
        public SenderProviderType FProviderType { get; set; }

        /// <summary>
        /// 报告事件类型
        /// </summary>
        public string FReportType { get; set; }

        /// <summary>
        /// 事件报告内容
        /// </summary>
        public string FReportContent { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime FCreateTime { get; set; }
    }
}