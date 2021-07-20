using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TchannelFreight
    {
        public long FchannelId { get; set; }
        public byte FfreightType { get; set; }
        public int FfirstWeight { get; set; }
        public decimal FfirstPrice { get; set; }
        public int FlimitWeight { get; set; }
        public decimal FoperateCost { get; set; }
        public string FfreightIntervals { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long FupdateBy { get; set; }
    }
}
