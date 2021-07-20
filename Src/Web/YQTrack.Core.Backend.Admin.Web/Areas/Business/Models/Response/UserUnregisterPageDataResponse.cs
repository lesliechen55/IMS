
using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{
    public class UserUnregisterPageDataResponse
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
        public byte NodeId { get; set; }
        public byte DbNo { get; set; }
        public byte TableNo { get; set; }
        public DateTime UnRegisterTime { get; set; }
        public DateTime CompletedTime { get; set; }
    }
}