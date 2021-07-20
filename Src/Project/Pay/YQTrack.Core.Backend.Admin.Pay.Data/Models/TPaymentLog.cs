using System;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TPaymentLog
    {
        public long FPaymentLogId { get; set; }
        public string FOrderId { get; set; }
        public long FUserId { get; set; }
        public string FAction { get; set; }
        public bool FSuccess { get; set; }
        public string FRequest { get; set; }
        public string FResponse { get; set; }
        public string FClientIP { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
