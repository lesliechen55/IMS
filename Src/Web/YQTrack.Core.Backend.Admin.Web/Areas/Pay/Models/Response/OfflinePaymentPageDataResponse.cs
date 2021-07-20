using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class OfflinePaymentPageDataResponse
    {
        public string OfflinePaymentId { get; set; }
        public DateTime CreateAt { get; set; }
        public string Email { get; set; }
        public string CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public decimal CalculateAmount { get; set; }
        public string TransferNo { get; set; }
        public string ProviderId { get; set; }
        public string Status { get; set; }
        public string HandleTime { get; set; }
        public string RejectReason { get; set; }

        public List<string> PurchaseOrderIdList { get; set; }
    }
}
