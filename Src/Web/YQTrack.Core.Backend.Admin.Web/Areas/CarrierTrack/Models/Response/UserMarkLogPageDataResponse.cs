using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response
{
    public class UserMarkLogPageDataResponse
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public DateTime CreateTime { get; set; }
    }
}