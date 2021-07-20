using System;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Output
{
    public class InquiryPageDataOutput
    {
        public long ForderId { get; set; }
        public string Ftitle { get; set; }
        public string FinquiryOrderNo { get; set; }
        public long FuserId { get; set; }
        public string FuserUniqueId { get; set; }
        public int FpackageCity { get; set; }
        public int FdeliveryCountry { get; set; }
        public DateTime FcreateTime { get; set; }
        public byte Fstatus { get; set; }
        public DateTime? FprocessTime { get; set; }
        public string FlogisticsRequire { get; set; }
        public string FcontactInfo { get; set; }
        public int FquoterCount { get; set; }
        public int FviewerCount { get; set; }
        public DateTime FexpireDate { get; set; }
        public DateTime FstatusTime { get; set; }
    }
}