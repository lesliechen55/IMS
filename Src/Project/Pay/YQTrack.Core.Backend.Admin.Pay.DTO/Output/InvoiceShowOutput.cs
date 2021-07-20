using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class InvoiceShowOutput
    {
        public long FInvoiceId { get; set; }
        public long FUserId { get; set; }
        public string FUserEmail { get; set; }
        public InvoiceType FInvoiceType { get; set; }
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
    }
}
