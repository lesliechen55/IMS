using System;

namespace YQTrack.Core.Backend.Admin.Log.Data.Models
{
    public partial class TStatisticDay
    {
        public long FStatisticId { get; set; }
        public int FStatisticType { get; set; }
        public int FType { get; set; }
        public int FCount { get; set; }
        public DateTime FStatisticDate { get; set; }
        public DateTime FCreateTime { get; set; }
        public DateTime FUpdateTime { get; set; }
    }
}
