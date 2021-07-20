using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class InvoiceApplyShowOutput
    {
        public long FInvoiceApplyId { get; set; }
        public long FUserId { get; set; }
        public string FEmail { get; set; }
        public DateTime FCreateAt { get; set; }
        public InvoiceType FInvoiceType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FAmount { get; set; }
        public InvoiceAuditStatus FStatus { get; set; }
        public SendType? FSendType { get; set; }
        public string FSendInfo { get; set; }
        public string FRejectReason { get; set; }
        public string FRemark { get; set; }

        public string FCompanyName { get; set; }
        public string FTaxNo { get; set; }
        public string FTaxPayerCertificateUrl { get; set; }
        public string FBank { get; set; }
        public string FBankAccount { get; set; }
        public string FAddress { get; set; }
        public string FTelephone { get; set; }
        public string FExpressAddress { get; set; }
        public string FInvoiceEmail { get; set; }
        public string FContact { get; set; }
        public string FPhone { get; set; }
        public List<InvoicePaymentOutput> InvoicePaymentList { get; set; }
    }

    public class InvoicePaymentOutput
    {
        public long FOrderId { get; set; }
        public string FOrderName { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FPaymentAmount { get; set; }
        public DateTime FPaymentCreateTime { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
    }
}