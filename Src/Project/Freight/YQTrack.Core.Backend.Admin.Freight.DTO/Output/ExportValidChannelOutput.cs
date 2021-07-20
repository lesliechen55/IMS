using System;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Output
{
    public class ExportValidChannelOutput
    {
        public string FchannelTitle { get; set; }
        public byte FproductType { get; set; }
        public byte FminDay { get; set; }
        public byte FmaxDay { get; set; }
        public string Fcountrys { get; set; }
        public int FlimitWeight { get; set; }
        public decimal FoperateCost { get; set; }
        public int FfirstWeight { get; set; }
        public decimal FfirstPrice { get; set; }
        public string FfreightIntervals { get; set; }
        public string FcompanyName { get; set; }
        public byte FfreightType { get; set; }
        public string FChannelType { get; set; }
        public int FchannelSubTypeId { get; set; }
        public DateTime? FpublishTime { get; set; }
    }
}