using System;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Output
{
    public class ChannelPageDataOutput
    {
        public string FchannelTitle { get; set; }
        public long FchannelId { get; set; }
        public byte FproductType { get; set; }
        public byte FminDay { get; set; }
        public byte FmaxDay { get; set; }
        public string Fcitys { get; set; }
        public string Fcountrys { get; set; }
        public int FlimitWeight { get; set; }
        public decimal FoperateCost { get; set; }
        public int FfirstWeight { get; set; }
        public decimal FfirstPrice { get; set; }
        public byte FfreightType { get; set; }
        public string FfreightIntervals { get; set; }
        public string FcompanyName { get; set; }
        public DateTime? FpublishTime { get; set; }
        public byte Fstate { get; set; }
        public DateTime FexpireTime { get; set; }
        public int FvalidReportTimes { get; set; }
    }
}