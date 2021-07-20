using System;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Output
{
    public class QuotePageDataOutput
    {
        public long FquoteId { get; set; }
        public string FquoteOrderNo { get; set; }
        public long ForderId { get; set; }
        public string FinquiryOrderNo { get; set; }
        public int FpackageCity { get; set; }
        public int FdeliveryCountry { get; set; }
        public byte Fstatus { get; set; }
        public long FuserId { get; set; }
        public long FcompanyId { get; set; }
        public string FcompanyName { get; set; }
        public string Fcontent { get; set; }
        public string Fremark { get; set; }
        public bool Fcancel { get; set; }
        public DateTime? FcancelTime { get; set; }
        public string FcancelReason { get; set; }
        public DateTime FcreateTime { get; set; }
        public bool Fviewed { get; set; }
    }
}