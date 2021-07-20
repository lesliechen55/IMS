namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public partial class ESDashboard
    {
        public int FPermissionId { get; set; }
        public string FDashboardSrc { get; set; }
        public int? FMaxDateRange { get; set; }
        public string FUsername { get; set; }
        public string FPassword { get; set; }
        public string FFieldsConfig { get; set; }
    }
}
