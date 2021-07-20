using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request
{
    public class EmailPageDataRequest : PageRequest
    {
        /// <summary>
        /// 邮件提交的开始时间
        /// </summary>
        [BindNever]
        public DateTime? SubmitStartTime { get; private set; }

        /// <summary>
        /// 邮件提交的结束时间
        /// </summary>
        [BindNever]
        public DateTime? SubmitEndTime { get; private set; }

        private string _submitDateRange;

        /// <summary>
        /// 时间范围
        /// </summary>
        public string SubmitDateRange
        {
            get => _submitDateRange;
            set
            {
                _submitDateRange = value;
                var (startDateTime, enDateTime) = DateHelper.Split(_submitDateRange);
                SubmitStartTime = startDateTime;
                SubmitEndTime = enDateTime;
            }
        }

        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 收件人邮箱
        /// </summary>
        public string To { get; set; }
    }
}