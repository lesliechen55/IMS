using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ReconcileItemShowResponse
    {
        public string OrderId { get; set; }
        public string ProviderId { get; set; }
        public string ProviderNo { get; set; }
        public string ProviderStatus { get; set; }
        public string ProviderType { get; set; }
        public string ProviderCurrency { get; set; }
        public string ProviderAmount { get; set; }
        public string ReconcileState { get; set; }
        public string Status { get; set; }
        public string Detail { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public bool IsTestOrder { get; set; } = false;
    }
}
