using System;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.Data.Entity
{
    public class OperationLog
    {
        public int FId { get; set; }
        public int FOperatorId { get; set; }
        public string FAccount { get; set; }
        public string FNickName { get; set; }
        public string FIp { get; set; }
        public string FMethod { get; set; }
        public string FParameter { get; set; }
        public string FDesc { get; set; }
        public OperationType FOperationType { get; set; }
        public DateTime FCreatedTime { get; set; } = DateTime.UtcNow;
        public int FCreatedBy { get; set; }
    }
}