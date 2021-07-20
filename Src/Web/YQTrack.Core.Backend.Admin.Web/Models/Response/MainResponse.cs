using System;

namespace YQTrack.Core.Backend.Admin.Web.Models.Response
{
    public class MainResponse
    {
        public int TotalManager { get; set; }
        public int TotalRole { get; set; }
        public int TotalPermission { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}