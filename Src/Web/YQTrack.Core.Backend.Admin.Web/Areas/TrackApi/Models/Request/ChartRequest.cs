using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request
{
    public class ChartRequest
    {
        public ChartDateType? ChartDateType { get; set; }
        public int? UserNo { get; set; }
        public string UserName { get; set; }
    }
}