using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IPaymentService : IScopeService
    {
        /// <summary>
        /// 获取订单分页列表
        /// </summary>
        /// <param name="input">订单列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<PaymentPageDataOutput> outputs, int total)> GetPageDataAsync(PaymentPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PaymentShowOutput> GetByIdAsync(long id);

        /// <summary>
        /// 根据ID获取订单是否可退款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(PaymentProvider provider, PaymentStatus status)> GetRefundByIdAsync(long id);
    }
}
