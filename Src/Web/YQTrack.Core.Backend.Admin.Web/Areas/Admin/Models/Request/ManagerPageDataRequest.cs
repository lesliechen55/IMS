using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class ManagerPageDataRequest : PageRequest
    {
        public string Account { get; set; }
        public string NickName { get; set; }
    }
}