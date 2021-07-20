using System;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    /// <summary>
    /// 询价单状态变更日志表
    /// </summary>
    public partial class TInquiryOrderStatusLog
    {
        public long FId { get; set; }
        public long FOrderId { get; set; }
        public InquiryOrderStatus FFrom { get; set; }
        public InquiryOrderStatus FTo { get; set; }
        public string FDesc { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
    }
}