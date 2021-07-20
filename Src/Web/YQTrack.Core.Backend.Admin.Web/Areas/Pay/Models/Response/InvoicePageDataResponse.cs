using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class InvoicePageDataResponse
    {
        public string InvoiceId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string InvoiceType { get; set; }
        public string CompanyName { get; set; }
        public string TaxNo { get; set; }
        public string Contact { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
