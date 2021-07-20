using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IOfflinePaymentService : IScopeService
    {
        /// <summary>
        /// 获取线下交易分页列表
        /// </summary>
        /// <param name="input">线下交易列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<OfflinePaymentPageDataOutput> outputs, int total)> GetPageDataAsync(OfflinePaymentPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OfflinePaymentShowOutput> GetByIdAsync(long id);

        /// <summary>
        /// 线下交易审核通过
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "线下交易审核通过", type: OperationType.Edit)]
        Task<long> PassAsync(OfflinePaymentPassInput input, int operatorId);

        /// <summary>
        /// 线下交易驳回
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "线下交易驳回", type: OperationType.Edit)]
        Task<long> RejectAsync(OfflinePaymentRejectInput input, int operatorId);
    }
}
