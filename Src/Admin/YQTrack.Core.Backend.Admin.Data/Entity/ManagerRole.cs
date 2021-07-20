using System;

namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public class ManagerRole
    {
        public int FManagerId { get; set; }
        public int FRoleId { get; set; }
        public DateTime FCreatedTime { get; set; } = DateTime.UtcNow;
        public int FCreatedBy { get; set; }
    }
}