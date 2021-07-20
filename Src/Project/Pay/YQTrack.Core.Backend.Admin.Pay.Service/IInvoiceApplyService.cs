using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IInvoiceApplyService : IScopeService
    {
        /// <summary>
        /// 获取发票申请分页列表
        /// </summary>
        /// <param name="input">发票申请列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<InvoiceApplyPageDataOutput> outputs, int total)> GetPageDataAsync(InvoiceApplyPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InvoiceApplyShowOutput> GetByIdAsync(long id);

        /// <summary>
        /// 发票申请审核通过
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "发票申请审核通过", type: OperationType.Edit)]
        Task<long> PassAsync(InvoiceApplyPassInput input, int operatorId);

        /// <summary>
        /// 发票申请驳回
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "发票申请驳回", type: OperationType.Edit)]
        Task<long> RejectAsync(InvoiceApplyRejectInput input, int operatorId);
    }
}
