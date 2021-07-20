namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class OfflinePaymentRejectRequest
    {
        public long OfflinePaymentId { get; set; }
        public string RejectReason { get; set; }
        public string Remark { get; set; }
    }
}
