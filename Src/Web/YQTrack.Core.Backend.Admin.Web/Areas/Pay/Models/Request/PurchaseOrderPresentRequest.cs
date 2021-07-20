namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class PurchaseOrderPresentRequest
    {
        public long OrderId { get; set; }
        public int SkuId { get; set; }
        public int Quantity { get; set; }
    }
}