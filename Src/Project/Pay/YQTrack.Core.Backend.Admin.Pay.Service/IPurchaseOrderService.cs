using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Backend.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IPurchaseOrderService : IScopeService
    {
        /// <summary>
        /// 获取订单分页列表
        /// </summary>
        /// <param name="input">订单列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<PurchaseOrderPageDataOutput> outputs, int total)> GetPageDataAsync(PurchaseOrderPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PurchaseOrderShowOutput> GetByIdAsync(long id);

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        Task<(Dictionary<int, string> output, string email)> GetSkuAsync(long userId, CurrencyType currencyType);

        /// <summary>
        /// 赠送商品
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="skuId"></param>
        /// <param name="quantity"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        [OperationTracePlus("赠送用户商品", OperationType.Edit)]
        Task<long> PresentAsync(long orderId, int skuId, int quantity, int operatorId);

        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <returns></returns>
        Task<long> GetOrderSerialNoAsync();
    }
}
