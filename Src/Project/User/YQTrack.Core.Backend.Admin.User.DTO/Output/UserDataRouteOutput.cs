using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.User.DTO.Output
{
    public class UserDataRouteOutput
    {
        public UserRoleType? FuserRole { get; set; }
        public byte? FnodeId { get; set; }
        public byte? FdbNo { get; set; }
        public byte? FtableNo { get; set; }
    }

    public class UserDataRoutePlusOutput
    {
        public UserRoleType? FuserRole { get; set; }
        public byte? FnodeId { get; set; }
        public byte? FdbNo { get; set; }
        public byte? FtableNo { get; set; }
        public long FuserId { get; set; }
    }
}
