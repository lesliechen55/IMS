namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class RefundRequest
    {
        public long OrderId { get; set; }
        public string Reason { get; set; }
    }
}
