using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TinquiryOrderView
    {
        public long FviewId { get; set; }
        public long ForderId { get; set; }
        public long? FuserId { get; set; }
        public byte? FuserRole { get; set; }
        public string Femail { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
    }
}
