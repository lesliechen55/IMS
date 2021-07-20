using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class LoginLogPageDataResponse
    {
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Ip { get; set; }
        public string Platform { get; set; }
        public string UserAgent { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}