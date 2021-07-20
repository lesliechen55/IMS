using Newtonsoft.Json;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class RolePermissionResponse
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 配置前端treeGrid js插件序列化checkbox状态字段
        /// </summary>
        [JsonProperty(PropertyName = "lay_is_checked")]
        public bool IsSelect { get; set; }
    }
}