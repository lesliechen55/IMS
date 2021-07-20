namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class InvoiceApplyRejectRequest
    {
        public long InvoiceApplyId { get; set; }
        public string Remark { get; set; }
        public string RejectReason { get; set; }
    }
}
