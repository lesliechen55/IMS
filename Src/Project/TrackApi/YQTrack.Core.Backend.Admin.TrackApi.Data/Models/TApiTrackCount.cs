using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data.Models
{
    public partial class TApiTrackCount
    {
        public long FUserId { get; set; }
        public DateTime FDate { get; set; }
        public byte FHour { get; set; }
        public byte FMinute { get; set; }
        public int FCount { get; set; }
    }
}
