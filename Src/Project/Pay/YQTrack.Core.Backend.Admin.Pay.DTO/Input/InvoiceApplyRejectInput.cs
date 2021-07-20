namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class InvoiceApplyRejectInput
    {
        public long FInvoiceApplyId { get; set; }
        public string FRemark { get; set; }
        public string FRejectReason { get; set; }
    }
}
