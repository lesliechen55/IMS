namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class RoleSetPermissionRequest
    {
        public int Id { get; set; }
        public int[] PermissionIdList { get; set; }
    }
}