using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class PurchaseOrderShowResponse
    {
        public long PurchaseOrderId { get; set; }
        public string UserPlatformType { get; set; }
        public string CurrencyType { get; set; }
        public string ServiceType { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string Status { get; set; }
        public List<PurchaseOrderItemResponse> PurchaseOrderItem { get; set; }

        public bool IsShowPresentPage { get; set; }
        public Dictionary<int, string> SkuDic { get; set; }
        public string UserEmail { get; set; }

        public string IsSubscriptionConflict { get; set; }

        public long OriginalOrderId { get; set; }
    }

    public class PurchaseOrderItemResponse
    {
        public string ProductSkuCode { get; set; }
        public string ProductSkuName { get; set; }
        public string CurrencyType { get; set; }
        public decimal SaleUnitPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime CreateAt { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
    }
}
