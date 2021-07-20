using System;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class InvoicePageDataOutput
    {
        public long FInvoiceId { get; set; }
        public long FUserId { get; set; }
        public string FUserEmail { get; set; }
        public InvoiceType FInvoiceType { get; set; }
        public string FCompanyName { get; set; }
        public string FTaxNo { get; set; }
        public string FTaxPayerCertificateUrl { get; set; }
        public string FContact { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime? FUpdateAt { get; set; }
    }
}
