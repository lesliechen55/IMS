using System;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TSerialNo
    {
        public int FType { get; set; }
        public int FNo { get; set; }
        public bool FIsProdEnv { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
