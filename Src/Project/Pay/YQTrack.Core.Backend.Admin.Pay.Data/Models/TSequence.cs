using System;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TSequence
    {
        public long FSequenceId { get; set; }
        public string FPrefix { get; set; }
        public long FSequenceNo { get; set; }
        public byte[] FRowVersion { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
