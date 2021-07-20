using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TchannelStatistics
    {
        public long FchannelId { get; set; }
        public int ValidReportTimes { get; set; }
        public DateTime? FcreateTime { get; set; }
        public long? FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
    }
}
