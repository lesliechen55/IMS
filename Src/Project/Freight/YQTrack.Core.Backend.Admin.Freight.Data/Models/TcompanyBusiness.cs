using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TcompanyBusiness
    {
        public long FcompanyId { get; set; }
        public string Fcitys { get; set; }
        public string Fprovinces { get; set; }
        public string FmainBusiness { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long FupdateBy { get; set; }
        public byte[] FupdateTimestamp { get; set; }
    }
}
