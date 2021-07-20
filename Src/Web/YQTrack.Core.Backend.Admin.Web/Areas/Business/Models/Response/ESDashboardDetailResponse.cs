using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Enums.Admin;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{

    public class ESDashboardDetailResponse
    {
        public ESDashboardResponse ESDashboard { get; set; }

        public IEnumerable<ESFieldResponse> TimeRanges { get; set; }

        public string[] Categories { get; set; }

        public Dictionary<int, string> DicESFieldType => EnumHelper.GetSelectItem<ESFieldType>();
    }
    public class ESDashboardResponse
    {
        public int PermissionId { get; set; }
        public string DashboardSrc { get; set; }
        public int? MaxDateRange { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FieldsConfig { get; set; }
    }
}