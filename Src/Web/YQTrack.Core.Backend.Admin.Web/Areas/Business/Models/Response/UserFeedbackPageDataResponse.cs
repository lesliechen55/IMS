using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{
    public class UserFeedbackPageDataResponse
    {
        public string FeedbackId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Feedback { get; set; }
        public string State { get; set; }
        public string ReplyContent { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? ReplyTime { get; set; }
        public string ReplyUserId { get; set; }
    }
}