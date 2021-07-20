using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class RolePermissionTreeItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonProperty(PropertyName = "pid")]
        public int PId { get; set; }
    }

    public class RolePermissionTreeResponse : PageResponse<RolePermissionTreeItemResponse>
    {
        public int[] CheckedIdList { get; set; }
    }
}