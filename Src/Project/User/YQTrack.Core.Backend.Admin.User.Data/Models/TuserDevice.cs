using System;

namespace YQTrack.Core.Backend.Admin.User.Data.Models
{
    public partial class TuserDevice
    {
        public long FuserDeviceId { get; set; }
        public long FuserId { get; set; }
        public string Flanguage { get; set; }
        public string FdeviceId { get; set; }
        public string FdeviceModel { get; set; }
        public string FpushProvider { get; set; }
        public string FpushToken { get; set; }
        public string Fostype { get; set; }
        public string Fosversion { get; set; }
        public string FappVersion { get; set; }
        public bool? FisPush { get; set; }
        public bool? FisValid { get; set; }
        public DateTime? FlastVisitTime { get; set; }
        public DateTime FcreateTime { get; set; }
        public long? FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
        public byte[] FtimeStamp { get; set; }
        public DateTime? FsearchTime { get; set; }
        public string FsessionId { get; set; }
    }
}
