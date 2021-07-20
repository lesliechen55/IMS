namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class RoleAddRequest
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Remark { get; set; }
    }
}