using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.User.Data.Models
{
    public partial class TuserInfo
    {
        public long FuserId { get; set; }
        public UserRoleType? FuserRole { get; set; }
        public byte? FnodeId { get; set; }
        public byte? FdbNo { get; set; }
        public byte? FtableNo { get; set; }
        public string FnickName { get; set; }
        public string Femail { get; set; }
        public byte Fstate { get; set; }
        public DateTime? FlastSignIn { get; set; }
        public DateTime? FcreateTime { get; set; }
        public long? FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
        public byte[] FtimeStamp { get; set; }
        public byte[] FpasswordHash { get; set; }
        public byte[] FpasswordSalt { get; set; }
        public int? FpasswordLevel { get; set; }
    }
}
