using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class TransactionQueryRequest
    {
        public PaymentProvider PaymentProvider { get; set; }
        public long? OrderId { get; set; }
        public string TradeNo { get; set; }
    }
}