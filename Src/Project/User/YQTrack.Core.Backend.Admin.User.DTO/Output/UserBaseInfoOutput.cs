using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.User.DTO.Output
{
    public class UserBaseInfoOutput
    {
        public long FUserId { get; set; }
        public UserRoleType? FUserRole { get; set; }
        public string FNickName { get; set; }
        public string FEmail { get; set; }
        public string FLanguage { get; set; }
    }
}
