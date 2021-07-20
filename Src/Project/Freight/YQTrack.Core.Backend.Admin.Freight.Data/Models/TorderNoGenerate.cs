using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TorderNoGenerate
    {
        public long FgenerateId { get; set; }
        public string Ftype { get; set; }
        public string Fprefix { get; set; }
        public int Fsequence { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
    }
}
