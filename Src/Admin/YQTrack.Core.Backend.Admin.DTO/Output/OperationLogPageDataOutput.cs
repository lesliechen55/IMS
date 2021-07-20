using System;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class OperationLogPageDataOutput
    {
        public string FAccount { get; set; }
        public string FNickName { get; set; }
        public string FIp { get; set; }
        public string FMethod { get; set; }
        public string FParameter { get; set; }
        public string FDesc { get; set; }
        public OperationType FOperationType { get; set; }
        public DateTime FCreatedTime { get; set; }
    }
}