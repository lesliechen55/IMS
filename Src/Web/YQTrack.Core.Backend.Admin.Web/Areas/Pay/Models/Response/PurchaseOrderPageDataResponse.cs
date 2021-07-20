using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class PurchaseOrderPageDataResponse
    {
        public string PurchaseOrderId { get; set; }
        public string UserPlatformType { get; set; }
        public string CurrencyType { get; set; }
        public string ServiceType { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }//by austin 21-07-12 add
        public string Status { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public string ProviderId { get; set; }
        public string Conflict { get; set; }

        public string OriginalOrderId { get; set; }
    }
}
