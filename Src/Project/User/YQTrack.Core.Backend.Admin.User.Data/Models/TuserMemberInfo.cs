using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.User.Data.Models
{
    public partial class TuserMemberInfo
    {
        public long FuserId { get; set; }
        public UserMemberType FmemberType { get; set; }
        public UserMemberLevel FmemberLevel { get; set; }
        public DateTime? FstartTime { get; set; }
        public DateTime? FexpiresTime { get; set; }
        public DateTime? FcreateTime { get; set; }
        public long? FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
    }
}
