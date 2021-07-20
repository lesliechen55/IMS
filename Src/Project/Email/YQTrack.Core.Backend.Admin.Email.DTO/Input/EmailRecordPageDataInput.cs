using System;

namespace YQTrack.Core.Backend.Admin.Email.DTO.Input
{
    /// <summary>
    /// 邮件记录分页数据输入
    /// </summary>
    public class EmailRecordPageDataInput : PageInput
    {
        /// <summary>
        /// 邮件提交的开始时间
        /// </summary>
        public DateTime SubmitStartTime { get; set; }

        /// <summary>
        /// 邮件提交的结束时间
        /// </summary>
        public DateTime SubmitEndTime { get; set; }

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