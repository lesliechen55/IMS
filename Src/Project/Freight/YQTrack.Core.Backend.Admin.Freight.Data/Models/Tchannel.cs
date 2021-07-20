using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class Tchannel
    {
        public long FchannelId { get; set; }
        public string FchannelTitle { get; set; }
        public string FchannelDesc { get; set; }
        public int FchannelSubTypeId { get; set; }
        public byte FproductType { get; set; }
        public byte FminDay { get; set; }
        public byte FmaxDay { get; set; }
        public decimal? FavgDay { get; set; }
        public decimal? FoneKgprice { get; set; }
        public long FcompanyId { get; set; }
        public string Fcountrys { get; set; }
        public string FtransTypes { get; set; }
        public byte Fstate { get; set; }
        public byte FcheckState { get; set; }
        public DateTime FexpireTime { get; set; }
        public DateTime? FpublishTime { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long FupdateBy { get; set; }
        public bool DelFlag { get; set; }
        public byte[] FupdateTimestamp { get; set; }
        public int FvalidReportTimes { get; set; }
        public bool? FisProcessed { get; set; }
    }
}
