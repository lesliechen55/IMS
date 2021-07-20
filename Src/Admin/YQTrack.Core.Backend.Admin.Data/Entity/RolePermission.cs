using System;

namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public class RolePermission
    {
        public int FRoleId { get; set; }
        public int FPermissionId { get; set; }
        public DateTime FCreatedTime { get; set; } = DateTime.UtcNow;
        public int FCreatedBy { get; set; }
    }
}