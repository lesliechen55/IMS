using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TInvoiceApply
    {
        public long FInvoiceApplyId { get; set; }
        public long FUserId { get; set; }
        public InvoiceType FInvoiceType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FAmount { get; set; }
        public InvoiceAuditStatus FStatus { get; set; }
        public DateTime? FHandleTime { get; set; }
        public SendType? FSendType { get; set; }
        public string FSendInfo { get; set; }
        public string FRejectReason { get; set; }
        public string FRemark { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long? FUpdateBy { get; set; }
        public DateTime? FUpdateAt { get; set; }
    }
}
