using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IChartService : IScopeService
    {
        /// <summary>
        /// 获取订单数量分析(维度：业务类型/货币类型/平台类型/订单状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ChartOutput> GetOrderCountAnalysisOutputAsync(OrderChartInput input);

        /// <summary>
        /// 订单金额分析(维度：业务类型/货币类型/平台类型/订单状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ChartOutput> GetOrderAmountAnalysisOutputAsync(OrderChartInput input);

        /// <summary>
        /// 交易笔数分析(维度：支付渠道/业务类型/货币类型/平台类型/支付状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ChartOutput> GetPaymentCountAnalysisOutputAsync(PaymentChartInput input);

        /// <summary>
        /// 交易金额分析(维度：支付渠道/业务类型/货币类型/平台类型/支付状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ChartOutput> GetPaymentAmountAnalysisOutputAsync(PaymentChartInput input);
    }
}