using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output
{
    public class IndexPageDataOutput
    {
        public long FControlId { get; set; }
        public long FUserId { get; set; }
        public string FEmail { get; set; }
        public bool FEnable { get; set; }
        public int FImportTodayLimit { get; set; }
        public int FExportTimeLimit { get; set; }
        public DateTime FCreateTime { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public DateTime? FLastAccessTime { get; set; }
    }
}