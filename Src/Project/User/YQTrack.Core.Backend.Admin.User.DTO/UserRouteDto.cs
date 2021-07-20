
using YQTrack.Backend.Models;

namespace YQTrack.Core.Backend.Admin.User.DTO
{
    public class UserRouteDto
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }

        public DataRouteModel DataRouteModel { get; set; }
    }
}
