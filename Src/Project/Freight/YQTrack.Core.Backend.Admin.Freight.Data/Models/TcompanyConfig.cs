using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TcompanyConfig
    {
        public long FcompanyId { get; set; }
        public short FmaxChannel { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long FupdateBy { get; set; }
    }
}
