using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class InvoiceApplyShowResponse
    {
        public long InvoiceApplyId { get; set; }
        public long UserId { get; set; }
        public string Email { get; set; }
        public DateTime CreateAt { get; set; }
        public string InvoiceType { get; set; }
        public string CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string SendType { get; set; }
        public string SendInfo { get; set; }
        public string RejectReason { get; set; }
        public string Remark { get; set; }

        public string CompanyName { get; set; }
        public string TaxNo { get; set; }
        public string TaxPayerCertificateUrl { get; set; }
        public string Bank { get; set; }
        public string BankAccount { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string ExpressAddress { get; set; }
        public string InvoiceEmail { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public List<InvoicePaymentResponse> InvoicePaymentList { get; set; }
    }

    public class InvoicePaymentResponse
    {
        public string OrderId { get; set; }
        public string OrderName { get; set; }
        public string CurrencyType { get; set; }
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public string Provider { get; set; }
        public DateTime PaymentCreateTime { get; set; }
    }
}
