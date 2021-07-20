using System.Collections.Generic;
using Newtonsoft.Json;

namespace YQTrack.Core.Backend.Admin.Web.Models.Request
{
    public class PermissionCheckRequest
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string PlatForm { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string PermissionCode { get; set; }
        public IEnumerable<string> PermissionCodeList => JsonConvert.DeserializeObject<List<string>>(PermissionCode);
    }
}