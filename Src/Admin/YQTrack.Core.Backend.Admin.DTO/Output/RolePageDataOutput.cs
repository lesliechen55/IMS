using System;

namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class RolePageDataOutput
    {
        public int FId { get; set; }
        public string FName { get; set; }
        public bool FIsActive { get; set; }
        public string FRemark { get; set; }
        public DateTime FCreatedTime { get; set; }
        public DateTime? FUpdatedTime { get; set; }
    }
}