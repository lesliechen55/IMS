namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class OfflinePaymentRejectInput
    {
        public long FOfflinePaymentId { get; set; }
        public string FRejectReason { get; set; }
        public string FRemark { get; set; }
    }
}
