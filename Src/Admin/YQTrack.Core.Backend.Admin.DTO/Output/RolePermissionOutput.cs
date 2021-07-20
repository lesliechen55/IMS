namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class RolePermissionOutput
    {
        public int FId { get; set; }
        public int? FParentId { get; set; }
        public string FName { get; set; }
        public string FFullName { get; set; }
        public string FUrl { get; set; }
        public string FRemark { get; set; }
        public bool IsSelect { get; set; }
    }
}