using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class InvoiceApplyPageDataResponse
    {
        public DateTime CreateAt { get; set; }
        public string InvoiceApplyId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string InvoiceType { get; set; }
        public string CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string HandleTime { get; set; }
        public string SendInfo { get; set; }
    }
}
