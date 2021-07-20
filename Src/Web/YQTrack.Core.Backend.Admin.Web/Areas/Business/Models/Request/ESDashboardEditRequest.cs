using System.Collections.Generic;
using YQTrack.Core.Backend.Enums.Admin;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request
{
    public class ESDashboardEditRequest
    {
        public int PermissionId { get; set; }
        public string DashboardSrc { get; set; }
        public int? MaxDateRange { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IEnumerable<ESFieldsConfigRequest> FieldsConfig { get; set; }
    }
    public class ESFieldsConfigRequest
    {
        public string FieldName { get; set; }
        public ESFieldType Type { get; set; }
        public string Category { get; set; }
        public bool IsValue { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
    }
}