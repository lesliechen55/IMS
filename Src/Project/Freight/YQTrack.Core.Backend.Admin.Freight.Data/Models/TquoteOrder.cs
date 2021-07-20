using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TquoteOrder
    {
        public long FquoteId { get; set; }
        public string FquoteOrderNo { get; set; }
        public long ForderId { get; set; }
        public string FinquiryOrderNo { get; set; }
        public long FuserId { get; set; }
        public long FcompanyId { get; set; }
        public string FcompanyName { get; set; }
        public string Fcontent { get; set; }
        public string Fremark { get; set; }
        public bool Fcancel { get; set; }
        public DateTime? FcancelTime { get; set; }
        public string FcancelReason { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
        public bool Fviewed { get; set; }
    }
}
