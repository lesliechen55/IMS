using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class ManagerPageDataResponse
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Account { get; set; }
        public bool IsLock { get; set; }
        public string Remark { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string Email { get; set; }
    }
}