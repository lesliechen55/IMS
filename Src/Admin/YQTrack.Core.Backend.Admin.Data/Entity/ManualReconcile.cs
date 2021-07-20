using System;

namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public class ManualReconcile
    {
        public int FId { get; set; }
        public string FFileName { get; set; }
        public string FFileMd5 { get; set; }
        public string FFilePath { get; set; }
        public int FYear { get; set; }
        public int FMonth { get; set; }
        public int FOrderCount { get; set; }
        public string FRemark { get; set; }
        public int FCreatedBy { get; set; }
        public DateTime FCreatedTime { get; set; } = DateTime.UtcNow;
    }
}