using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TInvoiceApplyPayment
    {
        public long FId { get; set; }
        public long FUserId { get; set; }
        public long FInvoiceApplyId { get; set; }
        public long FPaymentId { get; set; }
        public long FOrderId { get; set; }
        public string FOrderName { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FPaymentAmount { get; set; }
        public DateTime FPaymentCreateTime { get; set; }
    }
}
