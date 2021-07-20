using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class LoginLogPageDataRequest : PageRequest
    {
        public string Platform { get; set; }
        public string NickName { get; set; }
        public string Account { get; set; }
    }
}