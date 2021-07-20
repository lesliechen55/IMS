using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TUserMarkLog
    {
        public long FId { get; set; }
        public long FUserId { get; set; }
        public string FDescription { get; set; }
        public string FDetail { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
    }
}
