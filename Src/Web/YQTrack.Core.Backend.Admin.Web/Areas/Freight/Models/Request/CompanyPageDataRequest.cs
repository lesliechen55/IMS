using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request
{
    public class CompanyPageDataRequest : PageRequest
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public CompanyAuditState? Status { get; set; }
    }
}