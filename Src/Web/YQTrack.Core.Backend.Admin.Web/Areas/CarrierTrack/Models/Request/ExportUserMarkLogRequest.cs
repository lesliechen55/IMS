using System;
using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request
{
    public class ExportUserMarkLogRequest
    {
        public string Email { get; set; }

        [DisplayName("开始日期")]
        public DateTime? StartTime { get; set; }

        [DisplayName("结束日期")]
        public DateTime? EndTime { get; set; }
    }
}