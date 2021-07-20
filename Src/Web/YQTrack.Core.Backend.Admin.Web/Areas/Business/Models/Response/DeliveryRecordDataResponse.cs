using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{
    public class DeliveryRecordDataResponse
    {
        /// <summary>
        /// 邮件服务提供商类型
        /// </summary>
        public string ProviderType { get; set; }

        /// <summary>
        /// 报告事件类型
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// 事件报告内容
        /// </summary>
        public string ReportContent { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}