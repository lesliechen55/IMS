using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TOrderMark
    {
        public long FOrderMarkId { get; set; }
        public long FMarkId { get; set; }
        public long FTrackInfoId { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
    }
}
