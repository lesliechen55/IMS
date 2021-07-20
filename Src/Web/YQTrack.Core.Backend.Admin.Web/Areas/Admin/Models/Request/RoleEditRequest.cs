namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class RoleEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Remark { get; set; }
    }
}