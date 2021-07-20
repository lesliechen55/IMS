using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request
{
    public class UserFeedbackPageDataRequest : PageRequest
    {
        public long? UserId { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
    }
}