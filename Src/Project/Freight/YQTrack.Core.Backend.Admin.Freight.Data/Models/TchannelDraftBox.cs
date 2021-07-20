using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TchannelDraftBox
    {
        public long FchannelId { get; set; }
        public long FcompanyId { get; set; }
        public string FchannelTitle { get; set; }
        public string Fjson { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long FupdateBy { get; set; }
    }
}
