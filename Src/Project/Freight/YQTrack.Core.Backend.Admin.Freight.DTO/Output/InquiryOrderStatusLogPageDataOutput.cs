using System;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Output
{
    public class InquiryOrderStatusLogPageDataOutput
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