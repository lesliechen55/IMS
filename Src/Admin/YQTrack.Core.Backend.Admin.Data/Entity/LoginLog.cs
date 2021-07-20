using System;

namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public class LoginLog
    {
        public int FId { get; set; }
        public int FManagerId { get; set; }
        public string FAccount { get; set; }
        public string FNickName { get; set; }
        public string FIp { get; set; }
        public string FPlatform { get; set; }
        public string FUserAgent { get; set; }
        public DateTime FLoginTime { get; set; }
        public DateTime FCreatedTime { get; set; } = DateTime.UtcNow;
        public int FCreatedBy { get; set; }
    }
}