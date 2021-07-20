using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class InvoiceAddRequest
    {
        public long UserId { get; set; }
        public string UserEmail { get; set; }
        public InvoiceType InvoiceType { get; set; }
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
    }
}
