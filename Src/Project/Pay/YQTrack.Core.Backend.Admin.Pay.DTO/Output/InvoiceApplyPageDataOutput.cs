using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class InvoiceApplyPageDataOutput
    {
        public DateTime FCreateAt { get; set; }
        public long FInvoiceApplyId { get; set; }
        public string FCompanyName { get; set; }
        public long FUserId { get; set; }
        public string FEmail { get; set; }
        public InvoiceType FInvoiceType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FAmount { get; set; }
        public InvoiceAuditStatus FStatus { get; set; }
        public DateTime? FHandleTime { get; set; }
        public SendType? FSendType { get; set; }
        public string FSendInfo { get; set; }
        public string FRejectReason { get; set; }
    }
}
