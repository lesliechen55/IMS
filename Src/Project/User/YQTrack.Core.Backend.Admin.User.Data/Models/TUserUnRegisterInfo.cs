using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.User.Data.Models
{
    public partial class TUserUnRegisterInfo
    {
        public long FId { get; set; }
        public long FUserId { get; set; }
        public UserRoleType FUserRole { get; set; }
        public string FEmail { get; set; }
        public byte FNodeId { get; set; }
        public byte FDbNo { get; set; }
        public byte FTableNo { get; set; }
        public DateTime FUnRegisterTime { get; set; }
        public DateTime FCompletedTime { get; set; }
    }
}
