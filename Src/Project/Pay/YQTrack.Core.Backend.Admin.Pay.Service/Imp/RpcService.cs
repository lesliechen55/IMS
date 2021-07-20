using System.Threading.Tasks;
using YQTrack.SRVI.Payment.Enum;
using YQTrack.SRVI.Payment.Interface;
using YQTrack.SRVI.Payment.Model;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class RpcService : IRpcService
    {
        private readonly IPaymentRpcService _paymentRpcService;

        public RpcService(IPaymentRpcService paymentRpcService)
        {
            _paymentRpcService = paymentRpcService;
        }

        public async Task<PaymentResponse> TransactionQueryAsync(PaymentProvider paymentProvider, long orderId, string tradeNo, RequestContext context)
        {
            return await _paymentRpcService.TransactionQueryAsync(paymentProvider, orderId, tradeNo, context);
        }

        public async Task<RefundResponse> RefundAsync(long orderId, string reason)
        {
            return await _paymentRpcService.RefundAsync(orderId, reason);
        }
    }
}