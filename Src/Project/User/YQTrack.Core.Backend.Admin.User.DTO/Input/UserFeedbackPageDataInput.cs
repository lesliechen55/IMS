namespace YQTrack.Core.Backend.Admin.User.DTO.Input
{
    public class UserFeedbackPageDataInput : PageInput
    {
        public long? UserId { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
    }
}