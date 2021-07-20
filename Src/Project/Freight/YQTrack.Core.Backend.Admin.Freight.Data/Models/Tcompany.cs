using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class Tcompany
    {
        public long FcompanyId { get; set; }
        public long FuserId { get; set; }
        public string FcompanyName { get; set; }
        public string Flogo { get; set; }
        public string Finfo { get; set; }
        public string Fremark { get; set; }
        public int Fscale { get; set; }
        public string Furl { get; set; }
        public string Fimg { get; set; }
        public string Fcontact { get; set; }
        public string Ftelphone { get; set; }
        public string Fmobile { get; set; }
        public string Farea { get; set; }
        public string Faddress { get; set; }
        public string Femail { get; set; }
        public string Fqq { get; set; }
        public int FcheckState { get; set; }
        public DateTime? FcheckTime { get; set; }
        public string FcheckDesc { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long FupdateBy { get; set; }
        public byte[] FupdateTimestamp { get; set; }
        public string Fcode { get; set; }
        public int FchannelValidReportTimes { get; set; }
        public string FcheckDescHistory { get; set; }
    }
}
