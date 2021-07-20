using System;
using YQTrack.Backend.Email.Enums;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Email.DTO.Output
{
    /// <summary>
    /// 邮件发送记录输出
    /// </summary>
    public class EmailRecordPageDataOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long FSenderRecordId { get; set; }

        /// <summary>
        /// 邮件服务提供商类型
        /// </summary>
        public SenderProviderType FProviderType { get; set; }

        /// <summary>
        /// 邮件标题
        /// </summary>
        public string FTitle { get; set; }

        /// <summary>
        /// 发件地址
        /// </summary>
        public string FFrom { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        public string FTo { get; set; }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public EmailType FBusinessEmailType { get; set; }

        /// <summary>
        /// 邮件类型
        /// </summary>
        public MessageTemplateType FMessageEmailType { get; set; }

        /// <summary>
        /// 提交完成时间
        /// </summary>
        public DateTime FSubmitTime { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public string FMessageId { get; set; }

        /// <summary>
        /// 提交错误状态
        /// </summary>
        public SubmitFailureStatus FSubmitFailureStatus { get; set; }

        /// <summary>
        /// 投递结果反馈时间
        /// </summary>
        public DateTime? FDeliveryReportedTime { get; set; }

        /// <summary>
        /// 投递失败状态
        /// </summary>
        public DeliveryFailureStatus? FDeliveryFailureStatus { get; set; }

        /// <summary>
        /// 投递失败消息
        /// </summary>
        public string FDeliveryFailureMessage { get; set; }

        /// <summary>
        /// 业务邮件发送错误状态(通过发送结果计算得到，给业务系统)
        /// </summary>
        public BusinessEmailFailureStatus? FBusinessEmailFailureStatus { get; set; }

        /// <summary>
        /// 业务通知是否确认
        /// </summary>
        public bool FBusinessNotifyConfirmed { get; set; }

        /// <summary>
        /// 业务系统反馈通知确认时间
        /// </summary>
        public DateTime? FBusinessNotifyConfirmTime { get; set; }
    }
}