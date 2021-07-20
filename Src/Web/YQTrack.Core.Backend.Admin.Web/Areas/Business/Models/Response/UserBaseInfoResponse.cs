using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{
    public class UserBaseInfoResponse
    {
        public string UserId { get; set; }
        public UserRoleType? UserRole { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
    }
}