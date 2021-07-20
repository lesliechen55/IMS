using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response
{
    public class IndexPageDataResponse
    {
        public string ControlId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool Enable { get; set; }
        public int ImportTodayLimit { get; set; }
        public int ExportTimeLimit { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? OfflineDay { get; set; }

        public int AvailableTrackNum { get; set; }
        public int BuyTotalCount { get; set; }
    }
}