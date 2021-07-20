namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request
{
    public class CarrierTrackUserEditRequest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int ImportTodayLimit { get; set; }
        public int ExportTimeLimit { get; set; }
        public bool Enable { get; set; }
    }
}