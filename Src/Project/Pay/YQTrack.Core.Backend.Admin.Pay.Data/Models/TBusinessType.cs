using System;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TBusinessType
    {
        public int FBusinessTypeId { get; set; }
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public string FBackUrl { get; set; }
        public string FSuccessUrl { get; set; }
        public string FErrorUrl { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
