using System;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ManualReconcilePageDataOutput
    {
        public string FFileName { get; set; }
        public string FFileMd5 { get; set; }
        public int FYear { get; set; }
        public int FMonth { get; set; }
        public int FOrderCount { get; set; }
        public string FRemark { get; set; }
        public DateTime FCreatedTime { get; set; } 
    }
}