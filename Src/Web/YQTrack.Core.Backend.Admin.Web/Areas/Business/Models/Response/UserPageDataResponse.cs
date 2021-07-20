using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{
    public class UserPageDataResponse
    {
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public byte? NodeId { get; set; }
        public byte? DbNo { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public DateTime? LastSignIn { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateBy { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public int? IsPay { get; set; }
        public string Source { get; set; }
    }
}