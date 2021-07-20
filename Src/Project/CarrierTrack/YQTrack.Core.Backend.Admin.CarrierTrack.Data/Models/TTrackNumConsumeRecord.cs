using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TTrackNumConsumeRecord
    {
        public long FRecordId { get; set; }
        public long FUserId { get; set; }
        public long FTrackInfoId { get; set; }
        public string FTrackNo { get; set; }
        public long FCtrlId { get; set; }
        public bool? FIsConsume { get; set; }
        public bool FIsDelete { get; set; }
        public byte? FConsumeType { get; set; }
        public DateTime? FCreateTime { get; set; }
        public long? FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
    }
}
