using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TControl
    {
        public long FControlId { get; set; }
        public long FUserId { get; set; }
        public string FEmail { get; set; }
        public bool FEnable { get; set; }
        public int FImportTodayLimit { get; set; }
        public int FExportTimeLimit { get; set; }
        public DateTime? FLastAccessTime { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
    }
}
