using System;

namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class LoginLogPageDataOutput
    {
        public string FAccount { get; set; }
        public string FNickName { get; set; }
        public string FIp { get; set; }
        public string FPlatform { get; set; }
        public string FUserAgent { get; set; }
        public DateTime FLoginTime { get; set; }
        public DateTime FCreatedTime { get; set; }
    }
}