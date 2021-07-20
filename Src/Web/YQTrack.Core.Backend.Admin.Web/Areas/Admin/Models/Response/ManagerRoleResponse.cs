using Newtonsoft.Json;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class ManagerRoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }

        [JsonProperty(PropertyName = "LAY_CHECKED")]
        public bool IsSelect { get; set; }
    }
}