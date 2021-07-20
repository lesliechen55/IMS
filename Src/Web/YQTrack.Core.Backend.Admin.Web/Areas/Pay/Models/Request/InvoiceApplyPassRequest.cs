using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class InvoiceApplyPassRequest
    {
        public long InvoiceApplyId { get; set; }
        public SendType SendType { get; set; }
        public string SendInfo { get; set; }
        public string Remark { get; set; }
    }
}
