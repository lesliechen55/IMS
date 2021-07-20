using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public class Role : BaseEntity
    {
        public string FName { get; set; }
        public bool FIsActive { get; set; }
        public string FRemark { get; set; }
    }
}