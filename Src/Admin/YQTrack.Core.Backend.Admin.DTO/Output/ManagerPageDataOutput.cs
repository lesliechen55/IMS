using System;

namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class ManagerPageDataOutput
    {
        public int FId { get; set; }
        public string FNickName { get; set; }
        public string FAccount { get; set; }
        public bool FIsLock { get; set; }
        public string FRemark { get; set; }
        public string FAvatar { get; set; }
        public DateTime FCreatedTime { get; set; }
        public DateTime FLastLoginTime { get; set; }
        public DateTime? FUpdatedTime { get; set; }
        public string FEmail { get; set; }
    }
}