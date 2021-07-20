using System;

namespace YQTrack.Core.Backend.Admin.Core
{
    public abstract class BaseEntity : GenericEntity<int>
    {
        public DateTime FCreatedTime { get; set; } = DateTime.UtcNow;
        public int FCreatedBy { get; set; }
        public bool FIsDeleted { get; set; } = false;
        public DateTime? FUpdatedTime { get; set; }
        public int? FUpdatedBy { get; set; }
    }
}