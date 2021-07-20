using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TReport
    {
        public long FId { get; set; }
        public long FUserId { get; set; }
        public string FEmail { get; set; }
        public DateTime FDate { get; set; }
        public int FImport { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
    }
}
