namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request
{
    public class CarrierTrackUserAddRequest
    {
        public string Email { get; set; }
        public int ImportTodayLimit { get; set; }
        public int ExportTimeLimit { get; set; }
        public bool Enable { get; set; }
    }
}