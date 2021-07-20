using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ManualReconcilePageDataResponse
    {
        public string FileName { get; set; }
        public string FileMd5 { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int OrderCount { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}