using System;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class OperationLogPageDataRequest : PageRequest
    {
        public string Account { get; set; }
        public string Method { get; set; }
        public string Desc { get; set; }
        public OperationType? OperationType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}