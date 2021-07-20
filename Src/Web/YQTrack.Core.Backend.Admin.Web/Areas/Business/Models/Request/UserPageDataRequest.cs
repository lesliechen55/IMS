using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request
{
    public class UserPageDataRequest : PageRequest
    {
        public long? UserId { get; set; }
        public string Email { get; set; }

        public string Gid { get; set; }
    }
}