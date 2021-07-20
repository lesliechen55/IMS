using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{
    public class EmailPageDataResponse
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string SenderRecordId { get; set; }

        /// <summary>
        /// 邮件服务提供商类型
        /// </summary>
        public string ProviderType { get; set; }

        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发件地址
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 业务邮件模板
        /// </summary>
        public string BusinessEmailType { get; set; }

        /// <summary>
        /// 业务邮件模板
        /// </summary>
        public string MessageEmailType { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime SubmitTime { get; set; }

        /// <summary>
        /// 提交错误状态
        /// </summary>
        public string SubmitFailureStatus { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 投递结果反馈时间
        /// </summary>
        public DateTime? DeliveryReportedTime { get; set; }

        /// <summary>
        /// 投递失败状态
        /// </summary>
        public string DeliveryFailureStatus { get; set; }

        /// <summary>
        /// 投递失败消息
        /// </summary>
        public string DeliveryFailureMessage { get; set; }

        /// <summary>
        /// 业务邮件发送错误状态(通过发送结果计算得到，给业务系统)
        /// </summary>
        public string BusinessEmailFailureStatus { get; set; }

        /// <summary>
        /// 业务通知是否确认
        /// </summary>
        public string BusinessNotifyConfirmed { get; set; }

        /// <summary>
        /// 业务系统反馈通知确认时间
        /// </summary>
        public DateTime? BusinessNotifyConfirmTime { get; set; }
    }
}