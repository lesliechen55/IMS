using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request
{
    public class ChartRequest
    {
        public ChartDateType? ChartDateType { get; set; }
        public string Email { get; set; }
        public bool? Enable { get; set; }
    }
}