using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.SRVI.Payment.Enum;
using YQTrack.SRVI.Payment.Model;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IRpcService : ISingletonService
    {
        [OperationTracePlus(desc: "支付商交易查询", type: OperationType.Query)]
        Task<PaymentResponse> TransactionQueryAsync(PaymentProvider paymentProvider, long orderId, string tradeNo, RequestContext context);

        [OperationTracePlus(desc: "交易退款", type: OperationType.Edit)]
        Task<RefundResponse> RefundAsync(long orderId, string reason);
    }
}