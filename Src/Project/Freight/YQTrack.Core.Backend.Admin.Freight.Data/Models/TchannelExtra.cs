using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TchannelExtra
    {
        public long FchannelId { get; set; }
        public string FadditionalFees { get; set; }
        public string Fpiling { get; set; }
        public string FlongOverweight { get; set; }
        public string Fclaim { get; set; }
        public string Finvoice { get; set; }
        public string Freturn { get; set; }
        public string Fpackage { get; set; }
        public string Fother { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long FupdateBy { get; set; }
        public string FcombineRemark { get; set; }
    }
}
