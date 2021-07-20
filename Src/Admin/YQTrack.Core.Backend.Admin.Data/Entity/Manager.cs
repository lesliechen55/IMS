using System;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public class Manager : BaseEntity
    {
        public string FNickName { get; set; }
        public string FAccount { get; set; }
        public string FPassword { get; set; }
        public bool FIsLock { get; set; } = false;
        public string FRemark { get; set; }
        public string FAvatar { get; set; }
        public DateTime FLastLoginTime { get; set; }
        public string FEmail { get; set; }
    }
}