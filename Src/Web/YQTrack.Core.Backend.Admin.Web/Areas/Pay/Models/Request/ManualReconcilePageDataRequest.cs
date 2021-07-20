using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ManualReconcilePageDataRequest : PageRequest
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
    }
}