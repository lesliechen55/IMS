using System;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.DTO.Input
{
    public class OperationLogPageDataInput : PageInput
    {
        public string Account { get; set; }
        public string Method { get; set; }
        public string Desc { get; set; }
        public OperationType? OperationType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}