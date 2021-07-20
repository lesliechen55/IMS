using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class RolePageDataResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string RoleAccounts { get; set; }
    }
}