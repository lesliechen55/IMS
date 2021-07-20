using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class OperationLogPageDataResponse
    {
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Ip { get; set; }
        public string Method { get; set; }
        public string Parameter { get; set; }
        public string Desc { get; set; }
        public string OperationType { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}