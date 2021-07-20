using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TinquiryOrder
    {
        public long ForderId { get; set; }
        public long FuserId { get; set; }
        public string FuserUniqueId { get; set; }
        public string FinquiryOrderNo { get; set; }
        public byte Fstatus { get; set; }
        public DateTime FstatusTime { get; set; }
        public string Ftitle { get; set; }
        public int FpackageCity { get; set; }
        public int FdeliveryCountry { get; set; }
        public DateTime? FdeliveryDate { get; set; }
        public DateTime FexpireDate { get; set; }
        public string FlogisticsRequire { get; set; }
        public string Fother { get; set; }
        public string FcontactInfo { get; set; }
        public int FquoterCount { get; set; }
        public int FviewerCount { get; set; }
        public long? FloveCarrierId { get; set; }
        public long? FloveQuoteId { get; set; }
        public int? FstopSelfEnum { get; set; }
        public string FstopSelfReason { get; set; }
        public DateTime? FprocessTime { get; set; }
        public string FrejectReason { get; set; }
        public bool FdeleteFlag { get; set; }
        public bool FisNewQuote { get; set; }
        public DateTime? FautoStopTime { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
    }
}
