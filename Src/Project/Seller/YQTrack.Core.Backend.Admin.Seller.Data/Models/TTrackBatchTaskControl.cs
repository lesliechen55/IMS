using System;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Seller.Data.Models
{
    public partial class TTrackBatchTaskControl
    {
        public long FBatchId { get; set; }
        public string FSessionId { get; set; }
        public long FUserId { get; set; }
        public int FTaskType { get; set; }
        public DateTime FTaskStartTime { get; set; }
        public DateTime? FTaskEndTime { get; set; }
        public TrackBatchTaskStatus FTaskStatus { get; set; }
        public int FTotal { get; set; }
        public int? FSuccess { get; set; }
        public int? FError { get; set; }
        public int? FCurrentNum { get; set; }
        public int? FIgnoredNum { get; set; }
        public int? FBusinessStatus { get; set; }
        public bool FIsRead { get; set; }
        public DateTime FCreateTime { get; set; }
        public long? FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
        public DateTime? FTaskExpireTime { get; set; }
        public string FBusinessData { get; set; }
    }
}
