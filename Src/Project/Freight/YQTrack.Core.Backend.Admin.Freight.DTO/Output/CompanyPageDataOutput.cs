using System;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Output
{
    public class CompanyPageDataOutput
    {
        public long FcompanyId { get; set; }
        public long FuserId { get; set; }
        public string FcompanyName { get; set; }
        public string Fcontact { get; set; }
        public string Femail { get; set; }
        public string Fmobile { get; set; }
        public string Farea { get; set; }
        public string Faddress { get; set; }
        public string Furl { get; set; }
        public string FcheckDescHistory { get; set; }
        public DateTime FcreateTime { get; set; }
        public DateTime FupdateTime { get; set; }
        public int FcheckState { get; set; }
        public string Fcode { get; set; }
        public int FchannelValidReportTimes { get; set; }
    }
}