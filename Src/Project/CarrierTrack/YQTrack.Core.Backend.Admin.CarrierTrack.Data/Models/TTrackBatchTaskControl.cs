using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TTrackBatchTaskControl
    {
        public long FBatchId { get; set; }
        public long FUserId { get; set; }
        public int FTaskType { get; set; }
        public DateTime FTaskStartTime { get; set; }
        public DateTime? FTaskEndTime { get; set; }
        public short FTaskStatus { get; set; }
        public int FTotal { get; set; }
        public int? FSuccess { get; set; }
        public int? FError { get; set; }
        public int? FCurrentNum { get; set; }
        public bool FIsRead { get; set; }
        public DateTime FCreateTime { get; set; }
        public long? FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
    }
}
