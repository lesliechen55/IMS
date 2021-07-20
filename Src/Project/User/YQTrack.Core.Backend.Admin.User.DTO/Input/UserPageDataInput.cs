namespace YQTrack.Core.Backend.Admin.User.DTO.Input
{
    public class UserPageDataInput : PageInput
    {
        public long? UserId { get; set; }
        public string Email { get; set; }
    }
}