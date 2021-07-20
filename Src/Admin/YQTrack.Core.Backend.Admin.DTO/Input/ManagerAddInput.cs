namespace YQTrack.Core.Backend.Admin.DTO.Input
{
    public class ManagerAddInput
    {
        public string FNickName { get; set; }
        public string FAccount { get; set; }
        public string FPassword { get; set; }
        public bool FIsLock { get; set; }
        public string FRemark { get; set; }
        public string Email { get; set; }
    }
}