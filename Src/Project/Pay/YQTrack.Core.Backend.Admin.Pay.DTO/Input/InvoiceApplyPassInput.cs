using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class InvoiceApplyPassInput
    {
        public long FInvoiceApplyId { get; set; }
        public SendType FSendType { get; set; }
        public string FSendInfo { get; set; }
        public string FRemark { get; set; }
    }
}
